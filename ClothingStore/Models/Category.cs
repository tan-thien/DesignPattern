using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingStore.Models
{
    public class Category
    {
        [Key]
        public int IdCate { get; set; }

        
        public string CateName { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
