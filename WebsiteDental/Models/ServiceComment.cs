using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class ServiceComment
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int ServiceId { get; set; }

    public virtual Service Service { get; set; } = null!;
}
