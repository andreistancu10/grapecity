namespace DigitNow.Domain.DocumentManagement.Data.Entities.Objectives
{
    public class ObjectiveUploadedFile: ExtendedEntity
    {
        public long ObjectiveId { get; set; }
        public long UploadedFileId { get; set; }

        #region [ References ]

        public Objective Objective { get; set; }
        public BasicUploadedFile UploadedFile { get; set; }
        #endregion
    }
}
