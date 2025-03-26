using System.Collections.Generic;

namespace WebsiteDental.Models
{
    public class DoctorsDetailViewModel
    {
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
