using ClothingStore.Models;

namespace ClothingStore.Strategies
{
    public interface IOrderStatisticsStrategy
    {
        Task<Dictionary<string, (int Count, decimal TotalAmount)>> GetStatisticsAsync(IQueryable<Order> orders);

        IQueryable<Order> FilterOrdersByDateRange(IQueryable<Order> orders, DateTime? fromDate, DateTime? toDate);
    }
}
