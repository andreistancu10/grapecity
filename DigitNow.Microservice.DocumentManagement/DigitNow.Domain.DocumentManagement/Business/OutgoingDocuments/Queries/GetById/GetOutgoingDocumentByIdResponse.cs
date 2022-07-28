using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetById
{
    public class GetOutgoingDocumentByIdResponse
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public long ModifiedBy { get; set; }
        public int DepartmentId { get; set; }        
        public int DeadlineDaysNumber { get; set; }
        public string Description { get; set; }
        public string Observation { get; set; }
        public int ReceiverDepartmentId { get; set; }
        public bool? IsUrgent { get; set; }
        public int RegistrationNumber { get; set; }
        public string ContentSummary { get; set; }
        public int NumberOfPages { get; set; }
        public string RecipientName { get; set; }
        public int RecipientTypeId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentTypeDetail { get; set; }

        public ContactDetailDto ContactDetails { get; set; }
        public DeliveryDetailDto DeliveryDetails { get; set; }

        public List<WorkflowHistoryLogDto> WorkflowHistory { get; set; } = new();
        public List<ConnectedDocumentDto> ConnectedDocuments { get; set; } = new();
        public List<DocumentUploadedFileDto> DocumentUploadedFiles { get; set; } = new();
    }
}