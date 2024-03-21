﻿using Microsoft.AspNetCore.Mvc;
using Models.Contracts.V1.Blob;
using Models.Interfaces;

namespace blog_rest_api.Controllers.V1
{
    public class BlobController : Controller
    {
        [Route("blobs")]
        public class BlobExplorerController : Controller
        {
            private readonly IBlobService _blobService;

            public BlobExplorerController(IBlobService blobService)
            {
                _blobService = blobService;
            }

            [HttpGet("{blobName}")]
            public async Task<IActionResult> GetBlob(string blobName)
            {
                var data = await _blobService.GetBlobAsync(blobName);
                return File(data.Content, data.ContentType);
            }

            [HttpGet("list")]
            public async Task<IActionResult> ListBlobs()
            {
                return Ok(await _blobService.ListBlobsAsync());
            }

            [HttpPost("uploadfile")]
            public async Task<IActionResult> UploadFile([FromBody] UploadFileRequest request)
            {
                await _blobService.UploadFileBlobAsync(request.FilePath, request.FileName);
                return Ok();
            }

            [HttpPost("uploadcontent")]
            public async Task<IActionResult> UploadContent([FromBody] UploadContentRequest request)
            {
                await _blobService.UploadContentBlobAsync(request.Content, request.FileName);
                return Ok();
            }

            [HttpDelete("{blobName}")]
            public async Task<IActionResult> DeleteFile(string blobName)
            {
                await _blobService.DeleteBlobAsync(blobName);
                return Ok();
            }
        }
    }
}
