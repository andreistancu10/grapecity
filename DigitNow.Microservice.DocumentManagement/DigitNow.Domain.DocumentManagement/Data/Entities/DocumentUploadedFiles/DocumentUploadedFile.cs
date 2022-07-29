using DigitNow.Domain.DocumentManagement.Data.Entities.Documents;
using DigitNow.Domain.DocumentManagement.Data.Entities.UploadedFiles;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.DocumentUploadedFiles
{
    public class DocumentUploadedFile : ExtendedEntity
    {
        public long DocumentId { get; set; }
        public long UploadedFileId { get; set; }

        #region [ References ]

        public Document Document { get; set; }
        public UploadedFile UploadedFile { get; set; }
    
        #endregion
    }
}