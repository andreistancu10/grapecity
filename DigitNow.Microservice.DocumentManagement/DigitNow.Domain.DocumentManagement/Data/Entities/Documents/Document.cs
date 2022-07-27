using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System;
using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Data.Entities.DocumentUploadedFiles;
using DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisterMappings;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class Document : ExtendedEntity, IDocument
    {
        public DocumentType DocumentType { get; set; }
        public long RegistrationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public long? RecipientId { get; set; }
        public long DestinationDepartmentId { get; set; } 

        #region [ IDocument ]

        public DocumentStatus Status { get; set; }
        public DateTime StatusModifiedAt { get; set; }
        public long StatusModifiedBy { get; set; }

        #endregion

        #region [ Children ]

        public IncomingDocument IncomingDocument { get; set; }
        public InternalDocument InternalDocument { get; set; }
        public OutgoingDocument OutgoingDocument { get; set; }

        #endregion

        #region [ References ]

        public List<SpecialRegisterMapping> SpecialRegisterMappings { get; set; }
        public List<DocumentUploadedFile> DocumentUploadedFiles { get; set; }
        public List<WorkflowHistoryLog> WorkflowHistories { get; set; }

        #endregion
    }
}
