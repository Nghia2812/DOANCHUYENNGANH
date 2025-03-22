using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class AboutDental
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? Description { get; set; }

    public string? IconPath { get; set; }

    public string? ImagePath { get; set; }

    public int? StatValue1 { get; set; }

    public string? StatLabel1 { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsActive { get; set; }
}
