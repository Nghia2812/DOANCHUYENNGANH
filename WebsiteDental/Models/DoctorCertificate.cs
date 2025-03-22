using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class DoctorCertificate
{
    public int Id { get; set; }

    public int? DoctorId { get; set; }

    public string? CertificateName { get; set; }

    public string? IssuedBy { get; set; }

    public DateOnly? IssueDate { get; set; }

    public string? CertificateImage { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsActive { get; set; }

    public virtual Doctor? Doctor { get; set; }
}
