using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetWorkflowInformation
{
    public class GetWorkflowInformationByDocumentIdResponse
    {
        public DocumentStatus DocumentStatus { get; set; }
        public int UserRole { get; set; }
        public DateTime? OpinionRequestedUntil { get; set; }
    }
}
