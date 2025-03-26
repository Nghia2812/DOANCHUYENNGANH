using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class BlogComment
{
    public int Id { get; set; }

    public int? PostId { get; set; }

    public int? UserId { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsActive { get; set; }

    public string? Email { get; set; }

    public string? Img { get; set; }

    public string? Name { get; set; }

    public virtual BlogPost? Post { get; set; }

    public virtual User? User { get; set; }
}
