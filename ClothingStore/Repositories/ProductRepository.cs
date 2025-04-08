using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClothingStore.Models;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ClothingStoreContext _context;

        public ProductRepository(ClothingStoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
    .Include(p => p.Category) // Kết nối với bảng Category
    .FirstOrDefaultAsync(p => p.IdProduct == id);
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await Task.CompletedTask; // Hoặc thêm xử lý async nếu cần
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
        }
    }
}
