using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebsiteDental.Models
{
    public class AppointmentsDoctorsModelView
    {
        // Thông tin bác sĩ
        public int DoctorId { get; set; }  // ID bác sĩ
        public string DoctorName { get; set; } // Tên bác sĩ
        public string Specialization { get; set; } // Chuyên khoa
        public string ImageUrl { get; set; } // Ảnh bác sĩ

        // Thông tin lịch hẹn
        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày khám")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn giới tính")]
        public string Sex { get; set; } // Nam / Nữ

        public string Notes { get; set; } // Ghi chú

        // Nếu cần danh sách bác sĩ để chọn trong form
        public List<Doctor> Doctors { get; set; }
    }
}
