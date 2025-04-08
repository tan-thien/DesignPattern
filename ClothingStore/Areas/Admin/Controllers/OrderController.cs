using ClothingStore.Factories;
using ClothingStore.Models;
using ClothingStore.Observer;
using ClothingStore.Strategies;
using ClothingStore.Template;
using ClothingStore.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClothingStore.Controllers.Admin
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly OrderSubject _orderSubject;
        private readonly ClothingStoreContext _context;
        private IOrderStatisticsStrategy _strategy;

        public OrderController(IUnitOfWork unitOfWork, OrderSubject orderSubject, ClothingStoreContext context)
        {
            _unitOfWork = unitOfWork;
            _orderSubject = orderSubject;
            _context = context;
        }

        // Phương thức này có thể được gọi để thay đổi chiến lược trong controller nếu cần
        public void SetStrategy(IOrderStatisticsStrategy strategy)
        {
            _strategy = strategy;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _unitOfWork.OrderRepository.GetAllAsync();
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.StatusOrder = status;
            await _unitOfWork.CompleteAsync();

            // Gửi thông báo SMS
            await _orderSubject.NotifyObservers(order);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetStatistics(int? month, int? year)
        {
            // Nếu chưa chọn tháng và năm, mặc định sẽ lấy thống kê theo năm hiện tại và tháng hiện tại
            if (!month.HasValue) month = DateTime.Now.Month;
            if (!year.HasValue) year = DateTime.Now.Year;

            // Xác định ngày bắt đầu và kết thúc của tháng đã chọn
            var startOfMonth = new DateTime(year.Value, month.Value, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);  // Ngày cuối cùng của tháng

            // Lấy dữ liệu thống kê từ factory (dùng chiến lược MonthStatisticsStrategy)
            var strategy = OrderStatisticsFactory.GetStatisticsStrategy("Month");
            IQueryable<Order> query = _context.Orders;

            // Lọc theo ngày tháng
            query = strategy.FilterOrdersByDateRange(query, startOfMonth, endOfMonth);

            // Lấy thống kê
            var statistics = await strategy.GetStatisticsAsync(query);

            return View(statistics); // Trả về view với dữ liệu thống kê
        }


        // GET: Hiển thị form chọn ngày để thống kê theo ngày
        [HttpGet]
        public IActionResult GetStatisticsByDay()
        {
            // Lấy thống kê mặc định (ví dụ thống kê theo ngày hiện tại)
            var today = DateTime.Now;
            var startDate = today.Date;
            var endDate = today.Date;

            var strategy = OrderStatisticsFactory.GetStatisticsStrategy("Day");
            var orders = _context.Orders.AsQueryable();

            orders = strategy.FilterOrdersByDateRange(orders, startDate, endDate);
            var statistics = strategy.GetStatisticsAsync(orders).Result; // Sử dụng Result để đồng bộ cho ví dụ này

            // Truyền dữ liệu thống kê vào view
            return View(statistics);
        }

        [HttpPost]
        public async Task<IActionResult> GetStatisticsByDay(DateTime startDate, DateTime endDate)
        {
            if (startDate == DateTime.MinValue || endDate == DateTime.MinValue)
            {
                ViewBag.Error = "Vui lòng chọn ngày bắt đầu và kết thúc.";
                return View();
            }

            if (startDate > endDate)
            {
                ViewBag.Error = "Ngày bắt đầu không thể lớn hơn ngày kết thúc.";
                return View();
            }

            var strategy = OrderStatisticsFactory.GetStatisticsStrategy("Day");
            var orders = _context.Orders.AsQueryable();

            orders = strategy.FilterOrdersByDateRange(orders, startDate, endDate);
            var statistics = await strategy.GetStatisticsAsync(orders);

            return View("GetStatisticsByDay", statistics);
        }


        // Tương tự cho thống kê theo tháng
        [HttpGet]
        public IActionResult GetStatisticsByMonth()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetStatisticsByMonth(int month, int year)
        {
            if (month <= 0 || year <= 0)
            {
                ViewBag.Error = "Vui lòng chọn tháng và năm.";
                return View();
            }

            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var strategy = OrderStatisticsFactory.GetStatisticsStrategy("Month");
            var orders = strategy.FilterOrdersByDateRange(_context.Orders, startDate, endDate);
            var statistics = await strategy.GetStatisticsAsync(orders);

            return View("GetStatisticsByMonth", statistics);
        }

        // Tương tự cho thống kê theo năm
        [HttpGet]
        public IActionResult GetStatisticsByYear()
        {
            var statistics = new Dictionary<string, (int Count, decimal TotalAmount)>(); // Model mặc định
            return View(statistics);
        }

        [HttpPost]
        public async Task<IActionResult> GetStatisticsByYear(int year)
        {
            if (year <= 0)
            {
                ViewBag.Error = "Vui lòng chọn năm.";
                return View();
            }

            var startDate = new DateTime(year, 1, 1);
            var endDate = new DateTime(year, 12, 31);

            var strategy = OrderStatisticsFactory.GetStatisticsStrategy("Year");
            var orders = strategy.FilterOrdersByDateRange(_context.Orders, startDate, endDate);
            var statistics = await strategy.GetStatisticsAsync(orders);

            return View("GetStatisticsByYear", statistics);
        }




        public async Task<IActionResult> ExportStatisticsToJson(string period)
        {
            if (string.IsNullOrEmpty(period))
            {
                return BadRequest("Period cannot be null or empty.");
            }

            var statistics = await GetStatisticsData(period);
            if (statistics == null || !statistics.Any())
            {
                return NoContent(); // Return 204 No Content if no statistics are available
            }

            // Use JsonStatisticsExporter to generate the JSON file
            var jsonExporter = new JsonStatisticsExporter();
            byte[] fileBytes = await jsonExporter.ExportAsync(statistics);

            return File(fileBytes, "application/json", "OrderStatistics.json");
        }

        public async Task<IActionResult> ExportStatisticsToPdf(string period)
        {
            var statistics = await GetStatisticsData(period);
            if (statistics == null || !statistics.Any())
            {
                return BadRequest("Không có dữ liệu để xuất PDF.");
            }

            // Use PdfStatisticsExporter to generate the PDF file
            var pdfExporter = new PdfStatisticsExporter();
            byte[] fileBytes = await pdfExporter.ExportAsync(statistics);

            return File(fileBytes, "application/pdf", "ThongKe.pdf");
        }

        // 🔹 Method to get the statistics data (shared for both JSON and PDF exports)
        private async Task<Dictionary<string, (int Count, decimal TotalAmount)>> GetStatisticsData(string period)
        {
            if (string.IsNullOrEmpty(period))
            {
                Console.WriteLine("⚠️ Period is null or empty.");
                return null;
            }

            // Fetch the appropriate strategy for the period and get the statistics
            var strategy = OrderStatisticsFactory.GetStatisticsStrategy(period);
            var statistics = await strategy.GetStatisticsAsync(_context.Orders);

            // Log the fetched statistics
            if (statistics == null || !statistics.Any())
            {
                Console.WriteLine("⚠️ No statistics available.");
            }
            else
            {
                foreach (var entry in statistics)
                {
                    Console.WriteLine($"📌 Date: {entry.Key}, Count: {entry.Value.Count}, Total Amount: {entry.Value.TotalAmount}");
                }
            }

            return statistics;
        }




    }
}
