using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class ProductDetail
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public string? Specification { get; set; }

    public bool? IsActive { get; set; }

    public int? CategoryId { get; set; }

    public string? Slug { get; set; }

    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    public string? Ingredients { get; set; }

    public string? Usage { get; set; }

    public string? UsageInstructions { get; set; }

    public string? Preservation { get; set; }

    public string? Origin { get; set; }

    public string? Manufacturer { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public string? DeliveryInfo { get; set; }

    public string? WarrantyInfo { get; set; }

    public decimal? Price { get; set; }

    public decimal? OldPrice { get; set; }

    public int? DiscountPercent { get; set; }

    public string? ImageUrl { get; set; }

    public string? GalleryImages { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ProductCategory? Category { get; set; }

    public virtual Product? Product { get; set; }
}
