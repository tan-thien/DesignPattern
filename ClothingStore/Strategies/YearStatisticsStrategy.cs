using ClothingStore.Models;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Strategies
{
    public class YearStatisticsStrategy : IOrderStatisticsStrategy
    {
        public async Task<Dictionary<string, (int Count, decimal TotalAmount)>> GetStatisticsAsync(IQueryable<Order> orders)
        {
            return await orders
                .GroupBy(o => o.NgayDat.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    Count = g.Count(),
                    TotalAmount = g.Sum(o => (decimal)o.TongTien)
                })
                .ToDictionaryAsync(x => x.Year.ToString(), x => (x.Count, x.TotalAmount));
        }

        public IQueryable<Order> FilterOrdersByDateRange(IQueryable<Order> orders, DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate.HasValue)
                orders = orders.Where(o => o.NgayDat.Year >= fromDate.Value.Year);
            if (toDate.HasValue)
                orders = orders.Where(o => o.NgayDat.Year <= toDate.Value.Year);
            return orders;
        }
    }
}
