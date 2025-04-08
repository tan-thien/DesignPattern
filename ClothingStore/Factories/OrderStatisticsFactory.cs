using ClothingStore.Strategies;

namespace ClothingStore.Factories
{
    public class OrderStatisticsFactory
    {
        public static IOrderStatisticsStrategy GetStatisticsStrategy(string period)
        {
            return period.ToLower() switch
            {
                "year" => new YearStatisticsStrategy(),
                "month" => new MonthStatisticsStrategy(),
                "day" => new DayStatisticsStrategy(),
                _ => throw new ArgumentException("Invalid period"),
            };
        }
    }

}
