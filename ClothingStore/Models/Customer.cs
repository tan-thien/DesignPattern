using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingStore.Models
{
    public class Customer
    {
        [Key]
        public int IdCus { get; set; }
        public string CusName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        [ForeignKey("User")]
        public int IdUser { get; set; }
        public User? User { get; set; }

        public ICollection<Order> Orders { get; set; }

    }
}
