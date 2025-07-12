namespace SphereBlog.Models
{
    public class AuthToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ExpiresAt { get; set; }

        //Foreign keys
        public int UserId { get; set; }

        //Navigation property
        public User User { get; set; }

    }
}
