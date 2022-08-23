using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.UploadedFiles;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data
{
    public class UploadedFile : ExtendedEntity, IUploadedFile
    {
        public long UploadedFileMappingId { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string RelativePath { get; set; }
        public string AbsolutePath { get; set; }
        public Guid Guid { get; set; }

        #region References

        public UploadedFileMapping UploadedFileMapping { get; set; }

        #endregion
    }
}