using ClothingStore.Models;

namespace ClothingStore.Strategies
{
    public interface IAuthStrategy
    {
        Task<User?> LoginAsync(string username, string password);
    }
}
