using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class ContactForm
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Message { get; set; }

    public string? Phone { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public bool? IsActive { get; set; }
}
