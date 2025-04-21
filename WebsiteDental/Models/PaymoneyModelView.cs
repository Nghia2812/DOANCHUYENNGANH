using WebsiteDental.Models;
using WebsiteDental.ViewModels;

namespace WebsiteDental.Models
{
    public class PaymoneyModelView
    {
        // Các thuộc tính mới thêm vào


        public string UserId { get; set; } // Thêm UserId vào Model View
        public string username { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string Note { get; set; }

        // Các thuộc tính cũ
        public List<CartItemModelView> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TotalWithShipping { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
