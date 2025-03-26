using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class Appointment
{
    public int Id { get; set; }

    public int? PatientId { get; set; }

    public int? DoctorId { get; set; }

    public int? ServiceId { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public DateTime? AppointmentDate { get; set; }

    public string? Status { get; set; }

    public string? CancellationReason { get; set; }

    public string? Notes { get; set; }

    public bool? IsActive { get; set; }

    public string? CustomerName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Sex { get; set; }

    public string? NameDoctor { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual Patient? Patient { get; set; }
}
