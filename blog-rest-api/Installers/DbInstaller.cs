using blog_rest_api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace blog_rest_api.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<blog_rest_api.Data.DbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<blog_rest_api.Data.DbContext>();

            builder.Services.AddSingleton<IBlogService, BlogService>();
        }
    }
}
