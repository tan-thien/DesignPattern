using System.Text.Json;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace ClothingStore.Template
{
    public abstract class OrderStatisticsExporter
    {
        public async Task<byte[]> ExportAsync(Dictionary<string, (int Count, decimal TotalAmount)> statistics)
        {
            var formattedData = FormatData(statistics);
            return await Task.Run(() => GenerateFile(formattedData));
        }

        // Phương thức ghi đè để định dạng dữ liệu
        protected abstract string FormatData(Dictionary<string, (int Count, decimal TotalAmount)> statistics);

        // Phương thức ghi đè để xuất file
        protected abstract byte[] GenerateFile(string formattedData);
    }
}
