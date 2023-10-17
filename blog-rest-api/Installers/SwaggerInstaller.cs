namespace blog_rest_api.Installers
{
    public class SwaggerInstaller : IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Blog-API", Version = "v1" });
            });

        }
    }
}
