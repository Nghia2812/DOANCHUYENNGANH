using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class Discount
{
    public int Id { get; set; }

    public string? DiscountCode { get; set; }

    public decimal? DiscountPercentage { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool? IsActive { get; set; }

    public string? ImageUrl { get; set; }

    public string? OriginalPrice { get; set; }

    public string? FinalPrice { get; set; }

    public int? CategoryId { get; set; }

    public string? ProductCode { get; set; }

    public string? ServiceCode { get; set; }

    public string? ShippingCode { get; set; }

    public int? Quantity { get; set; }

    public virtual DiscountCategory? Category { get; set; }
}
