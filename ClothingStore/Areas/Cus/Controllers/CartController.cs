using ClothingStore.Models;
using ClothingStore.Service;
using ClothingStore.Session;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClothingStore.Areas.Cus.Controllers
{
    [Area("Cus")]
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(CartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId()
        {
            var userSession = SessionHelper.GetUser(HttpContext); // Lấy UserSessionModel
            int userId = userSession?.IdUser ?? 0; // Lấy IdUser từ session, nếu null thì trả về 0
            Console.WriteLine("Session UserId: " + userId); // Debug session
            return userId;
        }

        public IActionResult Index()
        {
            int userId = GetUserId();
            Console.WriteLine("userid: " + userId);
            if (userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            var cart = _cartService.GetCartByUserId(userId);
            ViewBag.TotalPrice = cart?.Items.Sum(i => i.Quantity * i.Price) ?? 0;
            return View(cart);
        }

        public IActionResult AddToCart(int productId, string productName, string productType, string imageUrl, decimal price, int stock)
        {
            int userId = GetUserId();
            Console.WriteLine($"[Checkout] UserId: {userId}");
            if (userId == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            _cartService.AddToCart(userId, productId, productName, productType, imageUrl, price, stock);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int productId)
        {
            int userId = GetUserId();
            _cartService.RemoveFromCart(userId, productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Checkout(int[] selectedProducts)
        {
            int userId = GetUserId();
            Console.WriteLine($"[Checkout] UserId: {userId}");

            if (userId == 0)
            {
                Console.WriteLine("[Checkout] User chưa đăng nhập. Chuyển hướng đến trang Login.");
                return RedirectToAction("Login", "Auth");
            }

            var cart = _cartService.GetCartByUserId(userId);
            if (cart == null)
            {
                Console.WriteLine($"[Checkout] Không tìm thấy giỏ hàng của UserId: {userId}");
                return RedirectToAction("Index");
            }

            Console.WriteLine($"[Checkout] Giỏ hàng có {cart.Items.Count} sản phẩm.");

            var selectedItems = cart.Items.Where(i => selectedProducts.Contains(i.ProductId)).ToList();

            if (!selectedItems.Any())
            {
                Console.WriteLine("[Checkout] Không có sản phẩm nào được chọn để thanh toán.");
                return RedirectToAction("Index");
            }

            Console.WriteLine($"[Checkout] Số lượng sản phẩm được chọn: {selectedItems.Count}");

            // Lưu vào OrderDetail tạm thời, chưa tạo Order
            var orderDetails = selectedItems.Select(i => new OrderDetail
            {
                IdProduct = i.ProductId,
                SoLuong = i.Quantity,
                Price = (int)i.Price
            }).ToList();

            Console.WriteLine("[Checkout] Lưu danh sách sản phẩm vào session:");
            foreach (var item in orderDetails)
            {
                Console.WriteLine($" - IdProduct: {item.IdProduct}, SoLuong: {item.SoLuong}, Price: {item.Price}");
            }

            HttpContext.Session.SetObjectAsJson("CheckoutItems", orderDetails);

            HttpContext.Session.SetInt32("UserId", userId);

            // Kiểm tra session ngay sau khi lưu
            var testSession = HttpContext.Session.GetObjectFromJson<List<OrderDetail>>("CheckoutItems");
            if (testSession == null)
            {
                Console.WriteLine("[Checkout] LỖI: Không lưu được session CheckoutItems!");
            }
            else
            {
                Console.WriteLine($"[Checkout] Session CheckoutItems đã lưu thành công! Số lượng: {testSession.Count}");

                // In chi tiết từng sản phẩm trong session
                Console.WriteLine("[Checkout] Dữ liệu trong session CheckoutItems:");
                foreach (var item in testSession)
                {
                    Console.WriteLine($" - IdProduct: {item.IdProduct}, SoLuong: {item.SoLuong}, Price: {item.Price}");
                }
            }

            var testUserId = HttpContext.Session.GetInt32("UserId");
            if (testUserId == null)
            {
                Console.WriteLine("[Checkout] LỖI: Không lưu được session UserId!");
            }
            else
            {
                Console.WriteLine($"[Checkout] Session UserId đã lưu thành công! Giá trị: {testUserId}");
            }

            Console.WriteLine("[Checkout] Chuyển hướng đến ConfirmOrder.");
            HttpContext.Session.CommitAsync().Wait();
            return Json(new { success = true, redirectUrl = Url.Action("ConfirmOrder", "Order", new { area = "Cus", userId = userId }) });

        }






        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            int userId = GetUserId();
            if (userId == 0) return RedirectToAction("Login", "Auth");

            _cartService.UpdateQuantity(userId, productId, quantity);
            return RedirectToAction("Index");
        }


    }
}