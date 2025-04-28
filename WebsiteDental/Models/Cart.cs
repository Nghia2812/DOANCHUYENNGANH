using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
    // Lấy giá từ đối tượng Product và tính tổng giá trị
    public decimal TotalPrice => (Product?.Price ?? 0) * (Quantity ?? 0);
}
