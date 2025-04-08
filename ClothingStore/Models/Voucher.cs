using System;
using System.ComponentModel.DataAnnotations;

namespace ClothingStore.Models
{
    public class Voucher
    {
        [Key]
        public int Id { get; set; }  // Khóa chính
        public string Code { get; set; }
        public double Discount { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
