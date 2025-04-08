using ClothingStore.Models;
using System;

namespace ClothingStore.Factory
{
    public class DefaultVoucherFactory : IVoucherFactory
    {
        public Voucher CreateVoucher()
        {
            return new Voucher
            {
                Code = GenerateRandomCode(),
                Discount = 0.15,
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(15)
            };
        }

        private string GenerateRandomCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
        }
    }
}
