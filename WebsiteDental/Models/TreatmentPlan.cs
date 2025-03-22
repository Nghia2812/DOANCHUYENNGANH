using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class TreatmentPlan
{
    public int Id { get; set; }

    public int? PatientId { get; set; }

    public int? DoctorId { get; set; }

    public string? PlanName { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EstimatedEndDate { get; set; }

    public decimal? TotalCost { get; set; }

    public string? PaymentStatus { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual Patient? Patient { get; set; }

    public virtual ICollection<TreatmentDetail> TreatmentDetails { get; set; } = new List<TreatmentDetail>();
}
