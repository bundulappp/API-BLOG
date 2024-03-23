using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.StaticFiles;
using Models.Interfaces;
using System.Text;
using BlobInfo = Models.Contracts.V1.Blob.BlobInfo;


namespace Logic.Services
{
    public class BlobService : IBlobService
    {
        private static readonly FileExtensionContentTypeProvider Provider = new FileExtensionContentTypeProvider();

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
            var contentStream = blobDownloadInfo.Value.Content.ToStream();
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
        public async Task UploadContentBlobAsync(string content, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            var bytes = Encoding.UTF8.GetBytes(content);
            await using var memoryStream = new MemoryStream(bytes);
            await blobClient.UploadAsync(memoryStream, new BlobHttpHeaders { ContentType = GetContentType(fileName) });
        }
        public async Task UploadFileBlobAsync(string filePath, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(filePath, new BlobHttpHeaders { ContentType = GetContentType(filePath) });
        }

        public Task DeleteBlobAsync(string blobName)
        {
            throw new NotImplementedException();
        }

        private string GetContentType(string fileName)
        {
            if (!Provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }
}
