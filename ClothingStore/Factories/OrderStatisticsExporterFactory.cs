using ClothingStore.Template;

namespace ClothingStore.Factories
{
    public static class OrderStatisticsExporterFactory
    {
        public static OrderStatisticsExporter GetExporter(string format)
        {
            return format.ToLower() switch
            {
                "json" => new JsonStatisticsExporter(),
                "pdf" => new PdfStatisticsExporter(),
                _ => throw new ArgumentException("Invalid format")
            };
        }
    }
}
