namespace blog_rest_api.Installers
{
    public interface IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder);
    }
}
