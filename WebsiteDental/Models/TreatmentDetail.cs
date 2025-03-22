using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class TreatmentDetail
{
    public int Id { get; set; }

    public int? PlanId { get; set; }

    public int? ServiceId { get; set; }

    public DateOnly? Date { get; set; }

    public string? Description { get; set; }

    public decimal? Cost { get; set; }

    public DateOnly? ScheduledDate { get; set; }

    public DateOnly? ActualDate { get; set; }

    public string? Notes { get; set; }

    public bool? IsActive { get; set; }

    public virtual TreatmentPlan? Plan { get; set; }

    public virtual Service? Service { get; set; }
}
