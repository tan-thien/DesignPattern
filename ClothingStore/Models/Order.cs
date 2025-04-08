using ClothingStore.Decorator;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClothingStore.Models
{
    public class Order
    {
        [Key]
        public int IdOrder { get; set; }

        [ForeignKey("Customer")]
        public int IdCus { get; set; }
        public Customer Customer { get; set; }
        public string StatusOrder { get; set; }
        public string? DiaChi { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime? NgayGiao { get; set; }
        public string? MoTa { get; set; }
        public int TongTien { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public string PaymentMethodName { get; set; }

        // Không ánh xạ trực tiếp BasePayment
        [NotMapped]
        public IPayment PaymentProcessor { get; set; }

        public string ProcessOrderPayment()
        {
            return PaymentProcessor?.ProcessPayment(TongTien) ?? "Phương thức thanh toán chưa được chọn!";
        }
    }




}
