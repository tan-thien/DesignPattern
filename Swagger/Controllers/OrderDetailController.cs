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
    public class OrderDetailController : ControllerBase
    {
        private readonly ClothingStoreContext _context;

        public OrderDetailController(ClothingStoreContext context)
        {
            _context = context;
        }

        // Lấy danh sách chi tiết đơn hàng
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOrderDetails()
        {
            var orderDetails = await _context.OrderDetails
                .Include(od => od.Order)
                .Include(od => od.Product)
                .ToListAsync();

            if (!orderDetails.Any())
            {
                return NotFound("Không có chi tiết đơn hàng nào.");
            }

            return Ok(orderDetails);
        }
    }
}
