using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class DocumentFileMapping : Entity
    {
        public long DocumentCategoryId { get; set; }
        public long UploadedFileMappingId { get; set; }

        #region

        public UploadedFileMapping UploadedFileMapping { get; set; }

        #endregion
    }
}