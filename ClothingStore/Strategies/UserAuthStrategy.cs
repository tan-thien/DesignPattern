using ClothingStore.Models;
using ClothingStore.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ClothingStore.Strategies
{
    public class UserAuthStrategy : IAuthStrategy
    {
        private readonly IUserRepository _userRepository;
        private readonly ClothingStoreContext _context;

        public UserAuthStrategy(IUserRepository userRepository, ClothingStoreContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _context.Users
                .Include(u => u.Account)
                .FirstOrDefaultAsync(u => u.UserName.Trim().Equals(username.Trim()) && u.PassWord == password);

            Console.WriteLine(user != null ? $"Đăng nhập thành công: {user.UserName}" : "Không tìm thấy người dùng hoặc mật khẩu sai!");

            if (user != null && user.PassWord == password && user.Account?.AccName == "User")
            {
                return user; 
            }
            return null;
        }

    }
}
