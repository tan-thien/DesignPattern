using System.Collections.Generic;
using System.Threading.Tasks;
using ClothingStore.Models;

namespace ClothingStore.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product); // Sửa lại cho đúng async
        void DeleteProduct(Product product);
    }
}
