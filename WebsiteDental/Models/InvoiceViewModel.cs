namespace WebsiteDental.Models
{
    public class InvoiceDetailWithProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? Quantity { get; set; }
        public decimal? FinalAmount { get; set; }
    }


    public class InvoiceViewModel
    {
        public int InvoiceId { get; set; }
        public DateOnly? OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }

        public User? User { get; set; }

        public List<InvoiceDetailWithProduct> Details { get; set; } = new List<InvoiceDetailWithProduct>();
    }


}
