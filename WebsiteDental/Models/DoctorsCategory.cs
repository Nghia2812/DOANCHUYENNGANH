using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class DoctorsCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Icon { get; set; }

    public string? Position { get; set; }

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
