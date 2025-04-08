using System.Text;
using System.Text.Json;

namespace ClothingStore.Template
{
    public class JsonStatisticsExporter : OrderStatisticsExporter
    {
        protected override string FormatData(Dictionary<string, (int Count, decimal TotalAmount)> statistics)
        {

            // Log dữ liệu trước khi chuyển đổi
            foreach (var entry in statistics)
            {
                Console.WriteLine($"Ngày: {entry.Key}, Số lượng: {entry.Value.Count}, Tổng tiền: {entry.Value.TotalAmount}");
            }
            // Chuyển đổi Tuple thành Dictionary với object có key-value rõ ràng
            var formattedData = statistics.ToDictionary(
                entry => entry.Key,
                entry => new { Count = entry.Value.Count, TotalAmount = entry.Value.TotalAmount }
            );

            return JsonSerializer.Serialize(formattedData, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        protected override byte[] GenerateFile(string formattedData)
        {
            return Encoding.UTF8.GetBytes(formattedData);
        }
    }
}
