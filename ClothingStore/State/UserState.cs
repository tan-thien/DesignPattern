namespace ClothingStore.State
{
    public class UserState : IAuthState
    {
        public void HandleRequest(HttpContext context)
        {
            context.Session.SetString("UserRole", "User");
        }
    }
}
