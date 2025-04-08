using ClothingStore.Models;

namespace ClothingStore.Observer
{
    public interface IOrderObserver
    {
        Task NotifyAsync(Order order);

    }
}
