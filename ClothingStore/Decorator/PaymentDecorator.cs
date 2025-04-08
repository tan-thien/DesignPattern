namespace ClothingStore.Decorator
{
    public class PaymentDecorator : IPayment
    {
        protected IPayment _payment;

        public PaymentDecorator(IPayment payment)
        {
            _payment = payment;
        }

        public virtual string ProcessPayment(int amount)
        {
            return _payment.ProcessPayment( amount);
        }

        public virtual int GetFinalPrice(int originalPrice)
        {
            return _payment.GetFinalPrice(originalPrice);
        }
    }

}
