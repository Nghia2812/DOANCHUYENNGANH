using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class Discount
{
    public int Id { get; set; }

    public string? DiscountCode { get; set; }

    public decimal? DiscountPercentage { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool? IsActive { get; set; }

    public string? ImageUrl { get; set; }
}
