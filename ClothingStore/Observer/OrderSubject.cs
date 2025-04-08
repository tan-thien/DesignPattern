using ClothingStore.Models;

namespace ClothingStore.Observer
{
    public class OrderSubject
    {
        private readonly List<IOrderObserver> _observers = new();

        public void Attach(IOrderObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IOrderObserver observer)
        {
            _observers.Remove(observer);
        }

        public async Task NotifyObservers(Order order)
        {
            foreach (var observer in _observers)
            {
                await observer.NotifyAsync(order);
            }
        }
    }

}
