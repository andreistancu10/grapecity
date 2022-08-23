using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class DocumentFileMapping : Entity
    {
        public long DocumentCategoryId { get; set; }
        public long UploadedFileId { get; set; }

        #region

        public UploadedFile UploadedFile{ get; set; }

        #endregion
    }
}