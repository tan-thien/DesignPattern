// IUserRepository.cs
using ClothingStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClothingStore.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);

        Task<User> GetUserWithCustomerAsync(int idUser);
        Task<User> RegisterAsync(User user);
        Task AddCustomerAsync(Customer customer);
    }
}
