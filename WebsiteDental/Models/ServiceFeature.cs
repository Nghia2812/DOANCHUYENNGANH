using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class ServiceFeature
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Icon { get; set; }

    public int Order { get; set; }

    public bool IsActive { get; set; }
}
