using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data.Documents
{
    public class DocumentResolution : ExtendedEntity
    {
        public long DocumentId { get; set; }

        public DocumentType DocumentType { get; set; }

        public DocumentResolutionType ResolutionType { get; set; }

        public string Remarks { get; set; }
    }
}
