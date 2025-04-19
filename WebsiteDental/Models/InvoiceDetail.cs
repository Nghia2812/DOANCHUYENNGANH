using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class InvoiceDetail
{
    public int Id { get; set; }

    public int? InvoiceId { get; set; }

    public int? ServiceId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? DiscountAmount { get; set; }

    public string? Notes { get; set; }

    public string? Description { get; set; }

    public decimal? TaxAmount { get; set; }

    public decimal? FinalAmount { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsActive { get; set; }

    public int? ProductId { get; set; }

    public virtual Invoice? Invoice { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Service? Service { get; set; }
}
