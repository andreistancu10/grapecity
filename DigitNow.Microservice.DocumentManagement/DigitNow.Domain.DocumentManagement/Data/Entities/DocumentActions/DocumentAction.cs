using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.DocumentActions
{
    public class DocumentAction : ExtendedEntity
    {
        public long DocumentId { get; set; }
        public UserActionsOnDocument Action { get; set; }
        public long ResposibleId { get; set; }
        public long? DepartmentId { get; set; }

        #region [ Relationships ]
        public Document Document { get; set; }

        #endregion
    }
}
