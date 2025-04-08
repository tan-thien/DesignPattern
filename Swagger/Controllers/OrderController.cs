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
    public class OrderController : ControllerBase
    {
        private readonly ClothingStoreContext _context;

        public OrderController(ClothingStoreContext context)
        {
            _context = context;
        }

        // Lấy danh sách đơn hàng
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ToListAsync();

            if (!orders.Any())
            {
                return NotFound("Không có đơn hàng nào.");
            }

            return Ok(orders);
        }

        // Lấy chi tiết đơn hàng theo IdOrder
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.IdOrder == id);

            if (order == null)
            {
                return NotFound("Không tìm thấy đơn hàng.");
            }

            return Ok(order);
        }
        // Lấy danh sách đơn hàng theo IdUser
        [HttpGet("user/{idUser}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUserId(int idUser)
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.Customer.IdUser == idUser)
                .OrderByDescending(o => o.NgayDat)
                .ToListAsync();

            if (!orders.Any())
            {
                return NotFound("Không tìm thấy đơn hàng nào cho người dùng này.");
            }

            return Ok(orders);
        }
    }
}
