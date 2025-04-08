namespace ClothingStore.State
{
    public class AdminState : IAuthState
    {
        public void HandleRequest(HttpContext context)
        {
            context.Session.SetString("UserRole", "Admin");
        }
    }
}
