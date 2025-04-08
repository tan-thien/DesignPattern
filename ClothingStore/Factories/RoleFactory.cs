namespace ClothingStore.Factories
{
    public class RoleFactory
    {
        public static IRole CreateRole(string roleName)
        {
            return roleName switch
            {
                "Admin" => new AdminRole(),
                "User" => new UserRole(),
                _ => new UserRole() // Mặc định là User
            };
        }
    }
}
