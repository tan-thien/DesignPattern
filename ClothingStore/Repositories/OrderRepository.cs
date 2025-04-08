using ClothingStore.Models;
using ClothingStore.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothingStore.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ClothingStoreContext _context;

        public OrderRepository(ClothingStoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.IdOrder == id);
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
        }

        public async Task DeleteAsync(Order order)
        {
            _context.Orders.Remove(order);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int idCus)
        {
            // Kiểm tra nếu idCus không hợp lệ
            if (idCus == 0)
            {
                Console.WriteLine($"[ERROR] IdCus không hợp lệ: {idCus}");
                return new List<Order>();
            }

            return await _context.Orders
                .Where(o => o.IdCus == idCus) // Lọc theo IdCus trực tiếp
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .OrderByDescending(o => o.NgayDat)
                .ToListAsync();
        }
    }
}
