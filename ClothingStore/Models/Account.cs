using System.ComponentModel.DataAnnotations;

namespace ClothingStore.Models
{
    public class Account
    {
        [Key]
        public int IdAcc { get; set; }
        public string AccName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
