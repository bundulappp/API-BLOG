namespace Models
{
    public class AppConfig
    {
        public string TemplateGeneratedFilesPath { get; set; }

        public int MaxAllowedFileSizeInMB { get; set; }

        public int MaxAllowedFileSizeInBytes => MaxAllowedFileSizeInMB * 1048576;

        public string FileStorePath { get; set; }
    }
}
