using ClothingStore.Models;
using ClothingStore.Session;
using ClothingStore.State;
using ClothingStore.Strategies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ClothingStore.Controllers
{
    [Area("Cus")]
    public class AuthController : Controller
    {
        private readonly ClothingStoreContext _context;
        private readonly AuthContext _authContext;
        private readonly AdminAuthStrategy _adminAuthStrategy;
        private readonly UserAuthStrategy _userAuthStrategy;

        public AuthController(AdminAuthStrategy adminAuthStrategy, UserAuthStrategy userAuthStrategy, ClothingStoreContext context)
        {
            _authContext = new AuthContext();
            _adminAuthStrategy = adminAuthStrategy;
            _userAuthStrategy = userAuthStrategy;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Kiểm tra tài khoản trong database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username && u.PassWord == password);
            if (user == null)
            {
                ViewData["ErrorMessage"] = "Tên đăng nhập hoặc mật khẩu không đúng!";
                return View();
            }

            // Chọn trạng thái phù hợp
            if (user.Role == "Admin")
            {
                _authContext.SetState(new AdminState());
            }
            else
            {
                _authContext.SetState(new UserState());
            }

            // Áp dụng trạng thái và lưu vào session
            _authContext.ApplyState(HttpContext);

            // Lưu thông tin user vào session
            var userSession = new UserSessionModel
            {
                IdUser = user.IdUser,
                UserName = user.UserName,
                Status = user.Status,
                IdAcc = user.IdAcc,
                Role = user.Role
            };
            SessionHelper.SetUser(HttpContext, userSession);

            // Điều hướng theo loại tài khoản
            if (user.Role == "Admin")
            {
                return RedirectToAction("Index", "Category", new { area = "Admin" });
            }
            return RedirectToAction("Index", "Product", new { area = "Cus" });
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Xóa toàn bộ session
            HttpContext.Session.Clear();

            // Điều hướng về trang đăng nhập
            return RedirectToAction("Login", "Auth");
        }

    }
}
