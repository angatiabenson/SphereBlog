namespace SphereBlog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public string Slug { get; set; }
        public bool IsPublished { get; set; } = false;

        // Foreign keys
        public int UserId { get; set; }

        //Navigation properties
        public User User { get; set; }
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Comments> Comments { get; set; } = new List<Comments>();
    }
}
