using ClothingStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClothingStore.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Order order);

        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);

    }
}
