using System;
using System.Collections.Generic;

namespace WebsiteDental.Models;

public partial class ImageGallery
{
    public int Id { get; set; }

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public DateTime? UploadedAt { get; set; }

    public bool? IsActive { get; set; }
}
