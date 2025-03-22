using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class Staff
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string FullName { get; set; } = null!;

    public DateOnly? Dob { get; set; }

    public string? Gender { get; set; }

    public string? Address { get; set; }

    public string? Department { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? IdNumber { get; set; }

    public string? ProfileImage { get; set; }

    public string? Position { get; set; }

    public decimal? Salary { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsActive { get; set; }

    public virtual User? User { get; set; }
}
