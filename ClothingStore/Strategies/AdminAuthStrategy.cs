using ClothingStore.Models;
using ClothingStore.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ClothingStore.Strategies
{
    public class AdminAuthStrategy : IAuthStrategy
    {
        private readonly IUserRepository _userRepository;
        private readonly ClothingStoreContext _context;

        public AdminAuthStrategy(IUserRepository userRepository, ClothingStoreContext context)
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

            if (user == null)
            {
                Console.WriteLine("Không tìm thấy người dùng!");
                return null;
            }

            Console.WriteLine($"Người dùng tìm thấy: {user.UserName}, Mật khẩu trong DB: {user.PassWord}, Mật khẩu nhập vào: {password}");

            if (user.PassWord == password && user.Account?.AccName == "Admin")
            {
                Console.WriteLine("Đăng nhập thành công với quyền Admin!");
                return user; 
            }

            return null;
        }

    }
}
