namespace WebsiteDental.Models
{
    public class DetailServicesModelView
    {
        public Service Service { get; set; }
        public List<BlogPost> BlogPosts { get; set; }

        public List<ServiceFeature> ServiceFeatures { get; set; }
    }
}
