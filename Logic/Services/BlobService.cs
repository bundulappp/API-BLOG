using Azure.Storage.Blobs;
using Models.Contracts.V1.Blob;
using Models.Interfaces;

namespace Logic.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private const string _blobContainerName = "blogs-image";
        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;

        }
        public async Task<BlobInfo> GetBlobAsync(string name)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerName);
            var blobClient = containerClient.GetBlobClient(name);
            var blobDownloadInfo = await blobClient.DownloadContentAsync();
            Stream contentStream = blobDownloadInfo.Value.Content.ToStream();
            return new BlobInfo(contentStream, blobDownloadInfo.Value.Details.ContentType);
        }
        public async Task<IEnumerable<string>> ListBlobsAsync()
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerName);
            var items = new List<string>();

            await foreach (var item in containerClient.GetBlobsAsync())
            {
                items.Add(item.Name);
            }
            return items;
        }
        public Task UploadFileBlobAsync(string filePath, string fileName)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBlobAsync(string blobName)
        {
            throw new NotImplementedException();
        }

        public Task UploadContentBlobAsync(string content, string fileName)
        {
            throw new NotImplementedException();
        }

    }
}
