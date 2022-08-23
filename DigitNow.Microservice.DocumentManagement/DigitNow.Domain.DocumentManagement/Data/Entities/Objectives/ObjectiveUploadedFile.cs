
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class ObjectiveUploadedFile : ExtendedEntity
    {
        public long ObjectiveId { get; set; }
        public long UploadedFileId { get; set; }

        #region [ References ]

        public Objective Objective { get; set; }
        public UploadedFile UploadedFile { get; set; }
        #endregion
    }
}
