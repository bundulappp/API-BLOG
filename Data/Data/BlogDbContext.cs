using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Domain;

namespace Data.Data
{
    public class BlogDbContext : IdentityDbContext
    {
        public BlogDbContext()
        {
        }
        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {
        }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }

    }
}