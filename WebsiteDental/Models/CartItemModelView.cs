namespace WebsiteDental.ViewModels
{
    public class CartItemModelView
    {

   
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Image { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
        public double? Rating { get; set; }

       
       public decimal TotalPrice => (Price ?? 0) * Quantity;
        //public decimal TotalPrice { get; set; }  // Thêm thuộc tính TotalPrice

    }
}
