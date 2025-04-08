
using ClothingStore.Models;
using ClothingStore.Factory;
using System.Collections.Generic;
using System.Linq;

namespace ClothingStore.Services
{
    public class VoucherService
    {
        private readonly ClothingStoreContext _context;
        private readonly IVoucherFactory _voucherFactory;

        public VoucherService(ClothingStoreContext context, IVoucherFactory voucherFactory)
        {
            _context = context;
            _voucherFactory = voucherFactory;
        }

        public void GenerateNewVoucher()
        {
            var newVoucher = _voucherFactory.CreateVoucher();
            _context.Vouchers.Add(newVoucher);
            _context.SaveChanges();
        }

        public List<Voucher> GetAllVouchers()
        {
            return _context.Vouchers.ToList();
        }
    }
}
