using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetWorkflowHistoryByDocumentId
{
    public class GetWorkflowHistoryByDocumentIdResponse
    {
        public string RecipientName { get; set; }
        public string Remarks { get; set; }
        public string DeclineReason { get; set; }
        public int? Resolution { get; set; }
        public DateTime? OpinionRequestedUntil { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
