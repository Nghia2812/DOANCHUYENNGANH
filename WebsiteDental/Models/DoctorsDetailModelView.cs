namespace WebsiteDental.Models
{
    public class DoctorsDetailViewModel
    {
        public Doctor Doctor { get; set; } // Thông tin bác sĩ
        public List<DoctorCertificate> Certificates { get; set; } // Danh sách chứng chỉ của bác sĩ

        // Constructor để tránh lỗi null khi chưa có dữ liệu
        public DoctorsDetailViewModel()
        {
            Certificates = new List<DoctorCertificate>();
        }
    }
}
