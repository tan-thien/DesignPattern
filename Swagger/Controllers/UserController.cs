using ClothingStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothingStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ClothingStoreContext _context;

        public UserController(ClothingStoreContext context)
        {
            _context = context;
        }

        // Lấy danh sách người dùng
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.Account)
                .Include(u => u.Customer)
                .ToListAsync();

            if (!users.Any())
            {
                return NotFound("Không có người dùng nào.");
            }

            return Ok(users);
        }

        // Lấy người dùng theo IdUser
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _context.Users
                .Include(u => u.Account)
                .Include(u => u.Customer)
                .FirstOrDefaultAsync(u => u.IdUser == id);

            if (user == null)
            {
                return NotFound("Không tìm thấy người dùng.");
            }

            return Ok(user);
        }
    }
}
