using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
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
        public ContactDetailDto ContactDetail { get; set; }
        public string IdentificationNumber { get; set; }
        public string ContentSummary { get; set; }
        public int NumberOfPages { get; set; }
        public int RecipientId { get; set; }
        public int DocumentTypeId { get; set; }
        public string Detail { get; set; }
        public double ResolutionPeriod { get; set; }
        public bool? IsUrgent { get; set; }
        public bool? IsGDPRAgreed { get; set; }
        public int RegistrationNumber { get; set; }
        public DeliveryDetailDto DeliveryDetails { get; set; }
        public List<WorkflowHistoryLogDto> WorkflowHistory { get; set; } = new();
        public List<ConnectedDocumentDto> ConnectedDocuments { get; set; } = new();
    }
}