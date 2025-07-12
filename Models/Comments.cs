namespace SphereBlog.Models
{
    public class Comments
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        //Foreign keys
        public int PostId { get; set; } 
        public int UserId { get; set; }

        // Navigation properties
        public Post Post { get; set; }
        public User User { get; set; }
    }
}
