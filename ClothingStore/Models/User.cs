using ClothingStore.Factories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingStore.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string? Status { get; set; }

        [ForeignKey("Account")]
        public int IdAcc { get; set; }
        public Account? Account { get; set; }

        // Thêm quan hệ 1-1 với Customer
        public Customer? Customer { get; set; }

        public string Role { get; set; } = "User"; // Mặc định là User, có thể thay đổi

        public IRole GetRoleInstance()
        {
            return RoleFactory.CreateRole(Role);
        }
    }
}
