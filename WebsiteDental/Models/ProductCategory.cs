using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class ProductCategory
{
    public int Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
