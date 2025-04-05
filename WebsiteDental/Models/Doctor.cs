using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class Doctor
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Specialization { get; set; }

    public int? ExperienceYears { get; set; }

    public string? Phone { get; set; }

    public string? LicenseNumber { get; set; }

    public decimal? ConsultationFee { get; set; }

    public decimal? Rating { get; set; }

    public string? Image { get; set; }

    public string? Biography { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsActive { get; set; }

    public string? Description { get; set; }

    public int? CategoryId { get; set; }

    public string? DoctorNumber { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual DoctorsCategory? Category { get; set; }

    public virtual ICollection<CommentDoctor> CommentDoctors { get; set; } = new List<CommentDoctor>();

    public virtual ICollection<DoctorCertificate> DoctorCertificates { get; set; } = new List<DoctorCertificate>();

    public virtual ICollection<TreatmentPlan> TreatmentPlans { get; set; } = new List<TreatmentPlan>();

    public virtual User? User { get; set; }
}
