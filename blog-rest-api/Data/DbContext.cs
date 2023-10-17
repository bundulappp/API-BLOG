using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace blog_rest_api.Data
{
    public class DbContext : IdentityDbContext
    {
        public DbContext(DbContextOptions<DbContext> options)
            : base(options)
        {
        }
    }
}