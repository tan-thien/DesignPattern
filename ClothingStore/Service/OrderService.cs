using ClothingStore.Models;

namespace ClothingStore.Service
{
    public class OrderService
    {
        private readonly ClothingStoreContext _context;

        public OrderService(ClothingStoreContext context)
        {
            _context = context;
        }

        public Order CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order;
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders
                .Where(o => o.IdOrder == orderId)
                .FirstOrDefault();
        }
    }
}
