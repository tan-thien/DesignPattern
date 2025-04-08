using ClothingStore.Models;

namespace ClothingStore.Decorator
{
    public class VoucherPayment : PaymentDecorator
    {
        private readonly Voucher _voucher;

        public VoucherPayment(IPayment payment, Voucher voucher) : base(payment)
        {
            _voucher = voucher;
        }

        public override string ProcessPayment(int amount)
        {
            if (_voucher != null && DateTime.Now >= _voucher.Start && DateTime.Now <= _voucher.End)
            {
                int discountedAmount = (int)(amount * (1 - _voucher.Discount ));
                return _payment.ProcessPayment(discountedAmount) + $" (Đã áp dụng voucher: {_voucher.Code}, Giảm {_voucher.Discount}%)";
            }
            return _payment.ProcessPayment(amount) + " (Không có voucher hợp lệ)";
        }

        public int GetFinalPrice(int originalPrice)
        {
            return (int)(originalPrice * (1 - _voucher.Discount)); // Tính tổng tiền sau giảm giá
        }
    }
}
