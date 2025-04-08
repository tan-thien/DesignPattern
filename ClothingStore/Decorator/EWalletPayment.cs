using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ClothingStore.Decorator
{
    public class EWalletPayment : IPayment
    {
        public string ProcessPayment(int amount)
        {
            return $"Thanh toán qua ví điện tử: {amount} VNĐ";
        }
        public int GetFinalPrice(int originalPrice)
        {
            return originalPrice; // Không có giảm giá
        }
    }
}
