using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.UploadedFiles;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data
{
    public class UploadedFile : ExtendedEntity, IUploadedFile
    {
        public UsageLocation UsageLocation { get; set; }
        public string UsageContext { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string RelativePath { get; set; }
        public string AbsolutePath { get; set; }
        public Guid Guid { get; set; }
    }
}