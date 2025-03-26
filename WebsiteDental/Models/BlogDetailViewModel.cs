namespace WebsiteDental.Models
{
    public class BlogDetailViewModel
    {
        public BlogPost BlogPost { get; set; } // Bài viết chính
        public List<BlogComment> Comments { get; set; } // bình luận bài viết
        public List<BlogPost> RelatedPosts { get; set; } // Danh sách bài viết liên quan

        // Constructor để tránh lỗi null khi chưa có dữ liệu
        public BlogDetailViewModel()
        {
            RelatedPosts = new List<BlogPost>();
        }
    }
}
