using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.UploadedFiles;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class UploadedFile : ExtendedEntity, IUploadedFile
    {
        public long DocumentCategoryId { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string RelativePath { get; set; }
        public string AbsolutePath { get; set; }
        public Guid Guid { get; set; }
    }
}