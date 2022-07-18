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
        public DocumentStatus Status { get; set; }
        public int RegistrationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }

        #region [ References ]

        public IncomingDocument IncomingDocument { get; set; }
        public InternalDocument InternalDocument { get; set; }
        public OutgoingDocument OutgoingDocument { get; set; }

        //TODO: Remove this
        public List<SpecialRegisterMapping> SpecialRegisterMappings { get; set; }
        public List<DocumentUploadedFile> DocumentUploadedFiles { get; set; }

        #endregion
    }
}
