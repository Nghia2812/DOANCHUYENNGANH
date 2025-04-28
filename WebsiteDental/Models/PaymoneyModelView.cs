using WebsiteDental.Models;
using WebsiteDental.ViewModels;

namespace WebsiteDental.Models
{
    public class PaymoneyModelView
    {
        // Các thuộc tính mới thêm vào


        public int? UserId { get; set; } // Thêm UserId vào Model View
        public string username { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string Notes { get; set; }

        // Các thuộc tính cũ
        public List<CartItemModelView> CartItems { get; set; }
        public decimal DiscountAmount { get; set; }        
        public int DiscountPercentage { get; set; }          
        public string DiscountCode { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TotalWithShipping { get; set; }
        public decimal TotalPrice { get; set; }

        // Thêm các thuộc tính sau để lưu trữ thông tin hóa đơn
        public WebsiteDental.Models.Invoice Invoice { get; set; }
        public List<WebsiteDental.Models.InvoiceDetail> InvoiceDetails { get; set; }
    }
}
