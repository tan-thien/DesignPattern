using ClothingStore.Models;
using ClothingStore.State;

namespace ClothingStore.Strategies
{
    public class AuthContext
    {

        private IAuthState _state;

        public void SetState(IAuthState state)
        {
            _state = state;
        }

        public void ApplyState(HttpContext context)
        {
            _state?.HandleRequest(context);
        }




        private IAuthStrategy _authStrategy;

        public void SetStrategy(IAuthStrategy authStrategy)
        {
            _authStrategy = authStrategy;
        }

        public async Task<User?> ExecuteLogin(string username, string password)
        {
            if (_authStrategy == null)
            {
                Console.WriteLine("Chiến lược đăng nhập chưa được thiết lập!");
                return null;
            }

            Console.WriteLine($"Đang thực hiện đăng nhập với chiến lược: {_authStrategy.GetType().Name}");
            var user = await _authStrategy.LoginAsync(username, password);

            if (user == null)
            {
                Console.WriteLine("Kết quả đăng nhập: Không tìm thấy người dùng hoặc mật khẩu không đúng!");
                return null;
            }

            Console.WriteLine($"Đăng nhập thành công! Người dùng: {user.UserName}");

            return user; 
        }


    }
}
