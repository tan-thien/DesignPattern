using System.Threading.Tasks;
using ClothingStore.Repositories;
using ClothingStore.Models;
using ClothingStore.Repository;

namespace ClothingStore.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClothingStoreContext _context;

        public ICategoryRepository Categories { get; }
        public IProductRepository Products { get; }
        public IUserRepository Users { get; }
        public IOrderRepository OrderRepository { get; private set; }


        public UnitOfWork(ClothingStoreContext context, IProductRepository productRepository, IUserRepository userRepository, IOrderRepository orderRepository)
        {
            _context = context;
            Categories = new CategoryRepository(context);
            Products = productRepository;
            Users = userRepository;
            OrderRepository = orderRepository;
        }

        public async Task CompleteAsync()
        {
            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
