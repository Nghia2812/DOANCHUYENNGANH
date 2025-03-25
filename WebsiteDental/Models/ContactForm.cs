using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebsiteDental.Models
{
    public partial class ContactForm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        public string? Message { get; set; }

        public DateTime? SubmittedAt { get; set; }

        public bool? IsActive { get; set; }
    }
}
