using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Models.Contracts.V1.Blob;
using Models.Interfaces;

namespace Logic.Services
{
    public class BlobService : IBlobService
    {
        public Task DeleteBlobAsync(string blobName)
        {
            throw new NotImplementedException();
        }

        public Task<BlobInfo> GetBlobAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> ListBlobsAsync()
        {
            throw new NotImplementedException();
        }

        public Task UploadContentBlobAsync(string content, string fileName)
        {
            throw new NotImplementedException();
        }

        public Task UploadFileBlobAsync(string filePath, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
