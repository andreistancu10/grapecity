using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models
{
    public class FileModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string OriginalFileName { get; set; }
        public long? TargetId { get; set; }
        public TargetEntity TargetEntity { get; set; }
        public string Context { get; set; }
        public string ContentType { get; set; }
    }
}