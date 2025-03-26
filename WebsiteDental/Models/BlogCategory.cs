using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class BlogCategory
{
    public int Id { get; set; }

    public string? CategoryName { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
}
