using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    //TODO: Fetch user & department data from cache once it's implemented
    public class WorkflowHistoryLog : ExtendedEntity
    {
        public long DocumentId { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public int DestinationDepartmentId { get; set; }
        public int RecipientType { get; set; } 
        public long RecipientId { get; set; }
        public string RecipientName { get; set; }
        public string Remarks { get; set; }
        public string DeclineReason { get; set; }
        public int? Resolution { get; set; }
        public DateTime? OpinionRequestedUntil { get; set; }

        #region [ Relationships ]

        public Document Document { get; set; }

        #endregion
    }
}
