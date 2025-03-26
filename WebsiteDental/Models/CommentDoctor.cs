using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class CommentDoctor
{
    public int Id { get; set; }

    public int DoctorId { get; set; }

    public string Username { get; set; } = null!;

    public string CommentText { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;
}
