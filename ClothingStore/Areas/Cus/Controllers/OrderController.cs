using ClothingStore.Decorator;
using ClothingStore.Models;
using ClothingStore.Session;
using ClothingStore.ViewModel;
using Microsoft.AspNetCore.Mvc;
using ClothingStore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ClothingStore.Areas.Cus.Controllers
{
    [Area("Cus")]
    public class OrderController : Controller
    {

        private readonly ClothingStoreContext _context;

        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork, ClothingStoreContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public IActionResult ConfirmOrder(string? voucherCode)
        {
            var orderDetails = HttpContext.Session.GetObjectFromJson<List<OrderDetail>>("CheckoutItems");
            if (orderDetails == null || !orderDetails.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            decimal discount = 0;
            if (!string.IsNullOrEmpty(voucherCode))
            {
                var voucher = _context.Vouchers
                    .FirstOrDefault(v => v.Code == voucherCode && v.Start <= DateTime.Now && v.End >= DateTime.Now);

                if (voucher != null)
                {
                    discount = (decimal)voucher.Discount;
                    ViewBag.VoucherMessage = $"Mã voucher hợp lệ! Giảm {discount * 100}%";
                }
                else
                {
                    ViewBag.VoucherMessage = "Mã voucher không hợp lệ!";
                }
            }


            var model = new ConfirmOrderViewModel { OrderDetails = orderDetails, Discount = discount };
            return View(model);
        }


        private int GetCurrentCustomerId()
        {
            int? idUser = HttpContext.Session.GetInt32("UserId");
            Console.WriteLine($"[ConfirmOrder] Giá trị UserId trong session: {idUser}");

            if (idUser == null)
            {
                throw new UnauthorizedAccessException("User chưa đăng nhập.");
            }

            // Truy vấn User kèm theo Customer
            var user = _unitOfWork.Users.GetUserWithCustomerAsync(idUser.Value).Result;


            if (user == null)
            {
                throw new Exception("Không tìm thấy người dùng.");
            }

            if (user.Customer == null)
            {
                throw new Exception("Không tìm thấy khách hàng tương ứng.");
            }

            return user.Customer.IdCus;
        }




        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string DiaChi, string MoTa, string PaymentMethod, int TongTien, string? VoucherCode)
        {
            Console.WriteLine($"[DEBUG] PaymentMethod nhận được: '{PaymentMethod}'");
            if (string.IsNullOrEmpty(PaymentMethod))
            {
                Console.WriteLine("[ERROR] PaymentMethod bị null hoặc rỗng!");
                throw new ArgumentException("Phương thức thanh toán không hợp lệ!");
            }

            string paymentMethodName = PaymentMethod switch
            {
                "1" => "Tiền mặt",
                "2" => "Ví điện tử",
                _ => throw new ArgumentException($"Phương thức thanh toán không hợp lệ! ({PaymentMethod})")
            };

            Console.WriteLine($"[DEBUG] Giá trị PaymentMethod sau khi chuyển đổi: '{paymentMethodName}'");

            // **Kiểm tra voucher trong đây**
            decimal discount = 0;
            if (!string.IsNullOrEmpty(VoucherCode))
            {
                var voucher = _context.Vouchers
                    .FirstOrDefault(v => v.Code == VoucherCode && v.Start <= DateTime.Now && v.End >= DateTime.Now);

                if (voucher != null)
                {
                    discount = (decimal)voucher.Discount; // Ép kiểu từ double sang decimal
                }
                else
                {
                    Console.WriteLine("[ERROR] Mã voucher không hợp lệ hoặc đã hết hạn!");
                }
            }


            var paymentProcessor = GetPaymentProcessor(PaymentMethod, VoucherCode);

            string paymentResult = paymentProcessor.ProcessPayment(TongTien);
            Console.WriteLine($"[DEBUG] Kết quả thanh toán: {paymentResult}");

            int finalPrice = ExtractFinalPrice(paymentResult);
            Console.WriteLine($"[DEBUG] Giá sau giảm: {finalPrice}");

            var order = new Order
            {
                IdCus = GetCurrentCustomerId(),
                DiaChi = DiaChi,
                MoTa = MoTa,
                StatusOrder = "Chờ xác nhận",
                NgayDat = DateTime.Now,
                TongTien = finalPrice,
                PaymentMethodName = PaymentMethod
            };

            await _unitOfWork.OrderRepository.AddAsync(order);
            await _unitOfWork.CompleteAsync();

            Console.WriteLine($"[DEBUG] OrderId mới tạo: {order.IdOrder}");

            var orderDetails = HttpContext.Session.GetObjectFromJson<List<OrderDetail>>("CheckoutItems");
            if (orderDetails == null || !orderDetails.Any())
            {
                Console.WriteLine("[ERROR] Không có sản phẩm nào trong giỏ hàng!");
                return RedirectToAction("Index", "Cart");
            }

            foreach (var item in orderDetails)
            {
                item.IdOrder = order.IdOrder;
                _context.OrderDetails.Add(item);
                Console.WriteLine($"[DEBUG] Thêm OrderDetail - IdProduct: {item.IdProduct}, SoLuong: {item.SoLuong}, Price: {item.Price}");
            }

            await _unitOfWork.CompleteAsync();

            TempData["Message"] = order.ProcessOrderPayment();

            _context.Carts.RemoveRange(_context.Carts.Where(c => c.UserId == GetCurrentCustomerId()));
            await _context.SaveChangesAsync();

            return RedirectToAction("OrderSuccess");
        }


        private IPayment GetPaymentProcessor(string paymentMethod, string? voucherCode)
        {
            if (string.IsNullOrEmpty(paymentMethod))
            {
                Console.WriteLine("[ERROR] paymentMethod bị null hoặc rỗng!");
                throw new ArgumentException("Phương thức thanh toán không hợp lệ!");
            }

            Console.WriteLine($"[DEBUG] Giá trị paymentMethod nhận được: '{paymentMethod}'");

            paymentMethod = paymentMethod.Trim() switch
            {
                "1" => "Tiền mặt",
                "2" => "Ví điện tử",
                _ => paymentMethod
            };

            IPayment paymentProcessor = paymentMethod switch
            {
                "Tiền mặt" => new CashOnDeliveryPayment(),
                "Ví điện tử" => new EWalletPayment(),
                _ => throw new ArgumentException($"Phương thức thanh toán không hợp lệ! ({paymentMethod})")
            };

            if (!string.IsNullOrEmpty(voucherCode))
            {
                voucherCode = voucherCode.Trim();
                var voucher = _context.Vouchers.FirstOrDefault(v => v.Code == voucherCode && v.Start <= DateTime.Now && v.End >= DateTime.Now);
                if (voucher != null)
                {
                    paymentProcessor = new VoucherPayment(paymentProcessor, voucher);
                    Console.WriteLine($"[DEBUG] Voucher hợp lệ: {voucher.Code}, Giảm {voucher.Discount * 100}%");
                }
                else
                {
                    Console.WriteLine("[ERROR] Mã voucher không hợp lệ!");
                }
            }
            return paymentProcessor;
        }


        public IActionResult OrderSuccess()
        {
            return View();
        }

        private int ExtractFinalPrice(string paymentResult)
        {
            var match = Regex.Match(paymentResult, @"\d+"); // Lấy số từ chuỗi
            return match.Success ? int.Parse(match.Value) : 0;
        }


    }
}
