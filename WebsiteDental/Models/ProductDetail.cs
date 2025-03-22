using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class ProductDetail
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public string? Specification { get; set; }

    public bool? IsActive { get; set; }

    public virtual Product? Product { get; set; }
}
