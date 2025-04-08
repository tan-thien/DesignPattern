using ClothingStore.Models;
using ClothingStore.Session;
using ClothingStore.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ClothingStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryController> _logger;


        public CategoryController(IUnitOfWork unitOfWork, ILogger<CategoryController> logger)
        {
            _unitOfWork = unitOfWork; // Dependency Injection
            _logger = logger;
        }

        // MVC: Hiển thị danh sách Categories
        public async Task<IActionResult> Index()
        {
            // Kiểm tra session user có tồn tại không
            var currentUser = SessionHelper.GetUser(HttpContext);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "Cus" });
            }

            // Kiểm tra quyền Admin
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Auth", new { area = "Cus" });
            }

            var categories = await _unitOfWork.Categories.GetAllAsync();
            return View(categories);
        }

        // Hiển thị form thêm mới Category
        public IActionResult Create()
        {
            // Kiểm tra session user có tồn tại không
            var currentUser = SessionHelper.GetUser(HttpContext);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "Cus" });
            }

            // Kiểm tra quyền Admin
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Auth", new { area = "Cus" });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            Console.WriteLine("Received Category:");
            Console.WriteLine($"IdCate: {category.IdCate}, CateName: {category.CateName}");

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("ModelState Error: " + error.ErrorMessage);
                }
                return View(category);
            }

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.CompleteAsync();

            Console.WriteLine("Category added successfully!");

            return RedirectToAction(nameof(Index));
        }


        // Hiển thị form chỉnh sửa Category
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return NotFound();

            Console.WriteLine($"Loading category: {category.IdCate}, Name: {category.CateName}");
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            try
            {
                var existingCategory = await _unitOfWork.Categories.GetByIdAsync(category.IdCate);
                if (existingCategory == null)
                {
                    return NotFound();
                }

                existingCategory.CateName = category.CateName;

                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra: " + ex.Message);
                return View(category);
            }
        }


        // Xoá Category
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return NotFound();

            _unitOfWork.Categories.DeleteAsync(category);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
