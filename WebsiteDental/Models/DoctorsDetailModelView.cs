using System.Collections.Generic;

namespace WebsiteDental.Models
{
    public class DoctorsDetailViewModel
    {
        public int DoctorId { get; set; }  // ID bác sĩ
        public Doctor Doctor { get; set; }
        public List<DoctorCertificate> Certificates { get; set; }
        public List<CommentDoctor> Comments { get; set; }

        public DoctorsDetailViewModel()
        {
            Certificates = new List<DoctorCertificate>();
            Comments = new List<CommentDoctor>();
        }
    }
}
