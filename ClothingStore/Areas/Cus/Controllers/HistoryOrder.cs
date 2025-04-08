using ClothingStore.Models;
using ClothingStore.Session;
using ClothingStore.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Areas.Cus.Controllers
{
    [Area("Cus")]
    public class HistoryOrder : Controller
    {
        private readonly ClothingStoreContext _context;

        private readonly IUnitOfWork _unitOfWork;

        public HistoryOrder(IUnitOfWork unitOfWork, ClothingStoreContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IActionResult> History()
        {
            int idCus = await GetCurrentUserIdAsync(); // Lấy IdCus thay vì UserId
            Console.WriteLine($"[DEBUG] Giá trị IdCus từ session: {idCus}");

            if (idCus == 0)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (idCus == 0) // Kiểm tra lại điều kiện, tránh null
            {
                throw new UnauthorizedAccessException("User chưa đăng nhập hoặc không có Customer.");
            }

            var orders = await _unitOfWork.OrderRepository.GetOrdersByUserIdAsync(idCus); // Truyền IdCus vào

            return View(orders);
        }

        // Xem chi tiết đơn hàng
        public async Task<IActionResult> Details(int id)
        {

            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        private async Task<int> GetCurrentUserIdAsync()
        {
            var userSession = SessionHelper.GetUser(HttpContext); // Lấy object UserSessionModel
            if (userSession == null || userSession.IdUser == 0) // Kiểm tra null và UserId
            {
                Console.WriteLine("[ERROR] Không tìm thấy UserId trong session hoặc giá trị là 0!");
                return 0;
            }

            Console.WriteLine($"[DEBUG] UserId từ session: {userSession.IdUser}");

            // Tìm IdCus từ bảng Customer
            var customerId = await _context.Customers
                .Where(c => c.IdUser == userSession.IdUser) // Sử dụng UserId thay vì .Value
                .Select(c => (int?)c.IdCus)
                .FirstOrDefaultAsync() ?? 0;

            if (customerId == 0)
            {
                Console.WriteLine($"[ERROR] Không tìm thấy Customer với IdUser = {userSession.IdUser}");
                return 0;
            }

            Console.WriteLine($"[DEBUG] IdCus từ UserId {userSession.IdUser}: {customerId}");
            return customerId;
        }
    }
}
