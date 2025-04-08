namespace ClothingStore.Decorator
{
    public class CashOnDeliveryPayment : IPayment
    {
        public string ProcessPayment(int amount)
        {
            return $"Thanh toán tiền mặt khi nhận hàng: {amount} VNĐ";
        }
        public int GetFinalPrice(int originalPrice)
        {
            return originalPrice; // Không có giảm giá
        }
    }
}
