using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class CustomerReview
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Customer { get; set; }

    public string? Email { get; set; }

    public int? Rating { get; set; }

    public string? Review { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsActive { get; set; }

    public string? ImageData { get; set; }

    public virtual User? User { get; set; }
}
