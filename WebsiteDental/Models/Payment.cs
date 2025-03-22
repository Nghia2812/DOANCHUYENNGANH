using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int? InvoiceId { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaymentStatus { get; set; }

    public string? TransactionId { get; set; }

    public bool? IsActive { get; set; }

    public int? ReceivedBy { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Invoice? Invoice { get; set; }
}
