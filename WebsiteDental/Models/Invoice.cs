using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class Invoice
{
    public int Id { get; set; }

    public int? PatientId { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateOnly? IssueDate { get; set; }

    public bool? IsPaid { get; set; }

    public bool? IsActive { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    public virtual Patient? Patient { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User? User { get; set; }
}
