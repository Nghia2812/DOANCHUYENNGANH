using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class ProductComment
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string? CommentText { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public bool IsActive { get; set; }

    public string? Gmail { get; set; }

    public virtual Product Product { get; set; } = null!;
}
