using ClothingStore.Repositories;
using ClothingStore.Session;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.Areas.Cus.Controllers
{
    [Area("Cus")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var user = SessionHelper.GetUser(HttpContext);
            Console.WriteLine("Session UserId in Index: " + (user?.IdUser ?? 0));

            var products = await _productRepository.GetAllProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
