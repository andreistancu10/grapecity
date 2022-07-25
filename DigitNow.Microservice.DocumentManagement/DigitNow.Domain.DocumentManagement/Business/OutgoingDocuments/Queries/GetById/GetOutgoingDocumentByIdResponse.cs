using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.DeliveryDetails;
using DigitNow.Domain.DocumentManagement.Data.Entities.DocumentUploadedFiles;
using System;
using System.Collections.Generic;

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
        public DeliveryDetail DeliveryDetails { get; set; }
        public List<WorkflowHistory> WorkflowHistory { get; set; } = new();
        public List<ConnectedDocument> ConnectedDocuments { get; set; } = new();
        public List<DocumentUploadedFile> DocumentUploadedFiles { get; set; } = new();
    }
}