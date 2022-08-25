namespace DigitNow.Domain.DocumentManagement.Business.Common.Models
{
    public class StoredFileModel : FileModel
    {
        public Guid GeneratedName { get; set; }
        public string RelativePath { get; set; }
        public string AbsolutePath { get; set; }
        public string ContentType { get; set; }
        public long CreatedBy { get; set; }
        public long UploadedFileMappingId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}