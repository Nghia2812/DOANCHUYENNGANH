using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class Revenue
{
    public int Id { get; set; }

    public DateOnly? Date { get; set; }

    public decimal? TotalAmount { get; set; }

    public bool? IsActive { get; set; }
}
