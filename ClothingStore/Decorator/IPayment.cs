
namespace ClothingStore.Decorator
{

        public interface IPayment
        {
            string ProcessPayment(int amount);
            int GetFinalPrice(int originalPrice); // Thêm phương thức này
    }

}
