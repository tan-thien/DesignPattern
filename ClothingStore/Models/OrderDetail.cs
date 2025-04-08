namespace ClothingStore.Models
{
    public class OrderDetail
    {
        public int IdOrder { get; set; }
        public Order Order { get; set; }
        public int IdProduct { get; set; }
        public Product Product { get; set; }
        public int SoLuong { get; set; }
        public int Price { get; set; }
    }
}
