using ClothingStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ClothingStoreContext _context;

        public UserController(ClothingStoreContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult SetRole(int userId, string roleName)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Cập nhật quyền của User
            user.Role = roleName;
            _context.SaveChanges();

            return RedirectToAction("UserList");
        }

        public IActionResult UserList()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
    }
}
