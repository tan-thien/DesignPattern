using ClothingStore.Models;

namespace ClothingStore.ViewModel
{
    public class ConfirmOrderViewModel
    {
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public int TongTien { get; set; } // Thêm thuộc tính này

        public string DiaChi { get; set; } = string.Empty; 

        public string MoTa { get; set; } = string.Empty; 

        public int PaymentMethod { get; set; } 

        public ConfirmOrderViewModel()
        {
            OrderDetails = new List<OrderDetail>();
        }

        public decimal Discount { get; set; }  // Thêm thuộc tính này
    }
}
