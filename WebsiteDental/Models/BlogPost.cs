using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class BlogPost
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public string? FeaturedImage { get; set; }

    public int? AuthorId { get; set; }

    public int? ViewCount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsActive { get; set; }

    public string? Description { get; set; }

    public virtual User? Author { get; set; }

    public virtual ICollection<BlogComment> BlogComments { get; set; } = new List<BlogComment>();

    public virtual BlogCategory? Category { get; set; }
}
