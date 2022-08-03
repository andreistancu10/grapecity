using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using System;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Queries.GetById
{
    public class GetInternalDocumentByIdResponse
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public long ModifiedBy { get; set; }
        public int SourceDepartmentId { get; set; }
        public int InternalDocumentTypeId { get; set; }
        public int DeadlineDaysNumber { get; set; }
        public string Description { get; set; }
        public string Observation { get; set; }
        public int DestinationDepartmentId { get; set; }
        public bool? IsUrgent { get; set; }
        public long RegistrationNumber { get; set; }
        public List<WorkflowHistoryLogDto> WorkflowHistory { get; set; } = new();
        public List<DocumentUploadedFileDto> DocumentUploadedFiles { get; set; } = new();
    }
}