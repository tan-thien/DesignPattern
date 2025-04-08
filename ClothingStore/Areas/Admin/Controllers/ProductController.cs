using ClothingStore.Models;
using ClothingStore.Repositories;
using ClothingStore.Session;
using ClothingStore.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace ClothingStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }

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
            var products = await _unitOfWork.Products.GetAllProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
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
            ViewBag.Categories = categories;
            ViewBag.Sizes = new List<string> { "S", "M", "L" };
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile ImageFile)
        {
            Console.WriteLine("====== Thông tin sản phẩm ======");
            Console.WriteLine("Product Name: " + product.ProductName);
            Console.WriteLine("Category Id: " + product.IdCate);
            Console.WriteLine("Price: " + product.Price);
            Console.WriteLine("Description: " + product.Description);
            Console.WriteLine("Size: " + product.Size);
            Console.WriteLine("Image Path: " + product.Image);

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                product.Image = "/images/" + fileName;
            }
            else
            {
                ModelState.AddModelError("Image", "Image is required.");
            }

            if (product.IdCate == 0)
            {
                ModelState.AddModelError("Category", "Category is required.");
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("Error: " + error.ErrorMessage);
                }

                var categories = await _unitOfWork.Categories.GetAllAsync();
                ViewBag.Categories = categories;
                ViewBag.Sizes = new List<string> { "S", "M", "L" };
                return View(product);
            }

            await _unitOfWork.Products.AddProductAsync(product);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }





        public async Task<IActionResult> Edit(int id)
        {
            var product = await _unitOfWork.Products.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Products.UpdateProductAsync(product);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Products.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _unitOfWork.Products.DeleteProduct(product);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction("Index");

        }
    }
}
