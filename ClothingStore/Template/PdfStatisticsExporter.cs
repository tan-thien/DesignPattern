using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.IO;
using ClothingStore.Template;

public class PdfStatisticsExporter : OrderStatisticsExporter
{
    // Ghi đè FormatData để tạo bố cục dễ nhìn
    protected override string FormatData(Dictionary<string, (int Count, decimal TotalAmount)> statistics)
    {
        var lines = new List<string> { "📊 THỐNG KÊ ĐƠN HÀNG" };
        foreach (var item in statistics)
        {
            lines.Add($"🔹 {item.Key}: {item.Value.Count} đơn - {item.Value.TotalAmount:N0} VNĐ");
        }
        return string.Join("\n", lines);
    }

    protected override byte[] GenerateFile(string formattedData)
    {
        using (var document = new PdfDocument())
        {
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var fontTitle = new XFont("Arial", 14, XFontStyle.Bold);
            var fontText = new XFont("Arial", 12, XFontStyle.Regular);

            int y = 30;
            var lines = formattedData.Split('\n');

            gfx.DrawString(lines[0], fontTitle, XBrushes.Black, new XPoint(50, y));
            y += 25;

            foreach (var line in lines.Skip(1))
            {
                gfx.DrawString(line, fontText, XBrushes.Black, new XPoint(50, y));
                y += 20;
            }

            using (var ms = new MemoryStream())
            {
                document.Save(ms, false);
                return ms.ToArray();
            }
        }
    }
}
