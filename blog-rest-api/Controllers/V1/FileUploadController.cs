using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using Models.Domain;
using Models.Interfaces;

namespace blog_rest_api.Controllers.V1
{
    [Route("api/file-upload")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {

        private readonly ILogger<FileUploadController> _logger;
        private readonly AppConfig _appConfig;
        private readonly IFileService _fileService;


        public FileUploadController(ILogger<FileUploadController> logger, IOptions<AppConfig> appConfig, IFileService fileService)
        {
            _logger = logger;
            _appConfig = appConfig.Value;
            _fileService = fileService;
        }


        [HttpGet("exists/{guid}")]
        public ActionResult<bool> IsFileExists(Guid guid)
            => Ok(_fileService.IsFileExists(guid));

        [HttpGet("{guid}")]
        [HttpGet("{guid}/{version}")]
        public IActionResult GetFile(Guid guid, uint? version)
        {
            var fileInfo = _fileService.GetFileInfo(guid, version);

            string mimeType = fileInfo.Extension switch
            {
                ".bmp" => "image/bmp",
                ".jpeg" or ".jpg" => "image/jpeg",
                ".png" => "image/png",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".pdf" => "application/pdf",
                _ => "application/octet-stream",
            };

            return File(_fileService.GetFile(guid, version), mimeType);
        }

        [HttpGet("info/{guid}/")]
        [HttpGet("info/{guid}/{version}")]
        public ActionResult<FileStore> GetFileInfo(Guid guid, uint? version)
            => Ok(_fileService.GetFileInfo(guid, version));

        [HttpPost]
        public ActionResult<Guid> FileUpload([FromForm] IFormFile sourceFile, [FromForm] Guid guid)
        {
            if (sourceFile.Length > this._appConfig.MaxAllowedFileSizeInBytes)
            {
                _logger.LogError("File is bigger than the maximum allowed!");
                return BadRequest($"File maximális megengedett mérete: {this._appConfig.MaxAllowedFileSizeInMB} MB");
            }

            var stream = sourceFile.OpenReadStream();

            try
            {
                return Ok(_fileService.UploadFile(stream, sourceFile.FileName, guid));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while trying to upload a file: {ex.Message}");
                return BadRequest("Hiba történt a file feltöltése közben!");
            }
        }

        [HttpDelete("{guid}/{latest}")]
        [Authorize(Policy = "RequireAdminOrSuperUserRole")]
        public ActionResult<bool> DeleteFile(Guid guid, bool latest)
            => Ok(_fileService.DeleteFile(guid, latest));
    }
}
