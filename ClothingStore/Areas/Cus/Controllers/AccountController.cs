using ClothingStore.Models;
using ClothingStore.Session;
using ClothingStore.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClothingStore.Controllers
{
    [Area("Cus")]
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Hiển thị trang đăng ký
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Xử lý đăng ký
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _unitOfWork.Users.GetByUsernameAsync(user.UserName);
                if (existingUser != null)
                {
                    ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại!");
                    return View(user);
                }

                await _unitOfWork.Users.RegisterAsync(user);
                return RedirectToAction("Login", "Auth", new { area = "Cus" });
                // Sau khi đăng ký thành công, chuyển hướng đến trang đăng nhập
            }
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // Lấy thông tin User từ session
            var userSession = SessionHelper.GetUser(HttpContext);

            if (userSession == null || userSession.IdUser == 0)
            {
                Console.WriteLine("[ERROR] Không tìm thấy UserId trong session!");
                return RedirectToAction("Login", "Auth"); // Chuyển hướng về trang đăng nhập
            }

            // Truy vấn user và customer
            var user = await _unitOfWork.Users.GetUserWithCustomerAsync(userSession.IdUser);

            if (user == null)
            {
                return NotFound("User không tồn tại!");
            }

            // Nếu chưa có thông tin Customer, hiển thị form cập nhật
            if (user.Customer == null)
            {
                return View("UpdateCustomerInfo", new Customer { IdUser = user.IdUser });
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomerInfo(Customer model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser = await _unitOfWork.Users.GetByIdAsync(model.IdUser);
            if (existingUser == null)
            {
                return NotFound("Không tìm thấy user!");
            }

            var newCustomer = new Customer
            {
                IdUser = model.IdUser,
                CusName = model.CusName,
                Phone = model.Phone,
                Address = model.Address
            };

            _unitOfWork.Users.AddCustomerAsync(newCustomer); // Cần thêm phương thức AddCustomer
            await _unitOfWork.CompleteAsync();

            return RedirectToAction("Profile");
        }
    }
}
