using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.Areas.User.Controllers
{
    [Area("Cus")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Store()
        {
            return View();
        }
        public IActionResult Gioithieu()
        {
            return View();
        }
    }
}
