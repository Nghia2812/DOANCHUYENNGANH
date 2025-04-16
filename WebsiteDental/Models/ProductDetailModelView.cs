using System.Collections.Generic;  // Để sử dụng List<>
using WebsiteDental.Models; // Để sử dụng các mô hình khác (Product, ProductCategory, ProductDetail)

namespace WebsiteDental.Models
{
    // ViewModel để chứa thông tin sản phẩm, danh mục và chi tiết
    public class ProductDetailModelView
    {
        // Thuộc tính chứa thông tin về sản phẩm
        public Product Product { get; set; }
        public List<ProductComment> Comments { get; set; } // bình luận bài viết
        // Thuộc tính chứa danh sách các danh mục (ProductCategory)
        public List<ProductCategory> CategoryList { get; set; }

        // Thuộc tính chứa chi tiết của sản phẩm (nếu có)
        public ProductDetail Detail { get; set; }
    }
}
