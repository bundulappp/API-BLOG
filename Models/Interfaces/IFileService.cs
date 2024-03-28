using Models.Domain;

namespace Models.Interfaces
{
    public interface IFileService
    {
        public bool IsFileExists(Guid fileId);

        public Stream GetFile(Guid fileId, uint? version = null);

        public FileStore GetFileInfo(Guid fileId, uint? version = null);

        public Guid UploadFile(Stream stream, string fileName, Guid? fileId = null);

        public bool DeleteFile(Guid fileId, bool latest = true);
    }
}
