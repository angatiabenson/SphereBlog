using Microsoft.EntityFrameworkCore;

namespace SphereBlog.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Post> Posts { get; set; }
        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Models.Comments> Comments { get; set; }
        public DbSet<Models.AuthToken> AuthTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.User>().ToTable("Users");
            modelBuilder.Entity<Models.Post>().ToTable("Posts");
            modelBuilder.Entity<Models.Category>().ToTable("Categories");
            modelBuilder.Entity<Models.Comments>().ToTable("Comments");
            modelBuilder.Entity<Models.AuthToken>().ToTable("AuthTokens");
            base.OnModelCreating(modelBuilder);
        }
    }
}
