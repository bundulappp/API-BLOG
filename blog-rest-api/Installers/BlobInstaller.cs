
using Azure.Storage.Blobs;

namespace blog_rest_api.Installers
{
    public class BlobInstaller : IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            string connectionString = builder.Configuration.GetValue<string>("AzureBlobStorageConnectionString");
            builder.Services.AddSingleton(x =>
              new BlobServiceClient(connectionString.Replace("AZURE-CONNECTION-STRING", Environment.GetEnvironmentVariable(connectionString))));
        }
    }
}
