using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class DiscountCategory
{
    public int Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Icon { get; set; }

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();
}
