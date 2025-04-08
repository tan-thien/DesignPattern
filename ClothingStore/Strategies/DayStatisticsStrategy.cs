using ClothingStore.Models;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Strategies
{
    public class DayStatisticsStrategy : IOrderStatisticsStrategy
    {
        public async Task<Dictionary<string, (int Count, decimal TotalAmount)>> GetStatisticsAsync(IQueryable<Order> orders)
        {
            var result = await orders
                .GroupBy(o => o.NgayDat.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count(),
                    TotalAmount = g.Sum(o => (decimal)o.TongTien) // ✅ Ép kiểu về decimal
                })
                .ToListAsync();

            return result.ToDictionary(x => x.Date.ToString("yyyy-MM-dd"), x => (x.Count, x.TotalAmount));
        }



        public IQueryable<Order> FilterOrdersByDateRange(IQueryable<Order> orders, DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate.HasValue)
                orders = orders.Where(o => o.NgayDat.Date >= fromDate.Value.Date);
            if (toDate.HasValue)
                orders = orders.Where(o => o.NgayDat.Date <= toDate.Value.Date);

            return orders;
        }

    }
}
