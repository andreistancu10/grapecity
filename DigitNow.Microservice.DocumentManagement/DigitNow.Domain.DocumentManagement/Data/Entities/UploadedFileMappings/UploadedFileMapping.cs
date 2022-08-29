using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data
{
    public class UploadedFileMapping : ExtendedEntity
    {
        public long TargetId { get; set; }
        public TargetEntity TargetEntity { get; set; }
        public long UploadedFileId { get; set; }

        #region [ References ]

        public UploadedFile UploadedFile { get; set; }

        #endregion
    }
}