using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class ServiceCategory
{
    public int Id { get; set; }

    public string? CategoryName { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public int? DisplayOrder { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
