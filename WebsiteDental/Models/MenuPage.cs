using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class MenuPage
{
    public int Id { get; set; }

    public string? PageName { get; set; }

    public string? Url { get; set; }

    public int? Position { get; set; }

    public bool? IsActive { get; set; }
}
