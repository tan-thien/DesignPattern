using ClothingStore.Models;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Strategies
{
    public class MonthStatisticsStrategy : IOrderStatisticsStrategy
    {
        public async Task<Dictionary<string, (int Count, decimal TotalAmount)>> GetStatisticsAsync(IQueryable<Order> orders)
        {
            return await orders
                .GroupBy(o => new { o.NgayDat.Year, o.NgayDat.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count(),
                    TotalAmount = g.Sum(o => (decimal)o.TongTien)
                })
                .ToDictionaryAsync(x => string.Format("{0}-{1:D2}", x.Year, x.Month), x => (x.Count, x.TotalAmount));
        }

        public IQueryable<Order> FilterOrdersByDateRange(IQueryable<Order> orders, DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate.HasValue)
                orders = orders.Where(o => o.NgayDat >= fromDate.Value);
            if (toDate.HasValue)
                orders = orders.Where(o => o.NgayDat <= toDate.Value);
            return orders;
        }

    }

}
