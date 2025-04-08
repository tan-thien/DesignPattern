using ClothingStore.Services;
using ClothingStore.Factory;
using Microsoft.AspNetCore.Mvc;
using ClothingStore.Models;


namespace ClothingStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VoucherController : Controller
    {
        private readonly VoucherService _voucherService;

        public VoucherController(VoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        public IActionResult Index()
        {
            var vouchers = _voucherService.GetAllVouchers();
            return View(vouchers);
        }

        public IActionResult Create()
        {
            _voucherService.GenerateNewVoucher();
            return RedirectToAction("Index");
        }
    }
}
