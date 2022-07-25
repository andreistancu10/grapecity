using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.DocumentUploadedFiles;
using System;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries.GetById
{
    public class GetIncomingDocumentByIdResponse
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public long ModifiedBy { get; set; }
        public int InputChannelId { get; set; }
        public int IssuerTypeId { get; set; }
        public string IssuerName { get; set; }
        public int ExternalNumber { get; set; }
        public DateTime? ExternalNumberDate { get; set; }
        public ContactDetail ContactDetail { get; set; }
        public string IdentificationNumber { get; set; }
        public string ContentSummary { get; set; }
        public int NumberOfPages { get; set; }
        public int RecipientId { get; set; }
        public int DocumentTypeId { get; set; }
        public string Detail { get; set; }
        public double ResolutionPeriod { get; set; }
        public bool? IsUrgent { get; set; }
        public bool? IsGDPRAgreed { get; set; }
        public List<WorkflowHistory> WorkflowHistory { get; set; } = new();
        public List<ConnectedDocument> ConnectedDocuments { get; set; } = new();
    }
}