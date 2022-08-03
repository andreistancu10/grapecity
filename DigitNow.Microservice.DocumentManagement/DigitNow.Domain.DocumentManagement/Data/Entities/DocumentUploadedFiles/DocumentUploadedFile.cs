namespace DigitNow.Domain.DocumentManagement.Data.Entities
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