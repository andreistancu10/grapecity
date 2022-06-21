using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.Documents.Models
{
    public class SetDocumentsResolutionRequest
    {
        public DocumentBatch Batch { get; set; }

        public DocumentResolutionType Resolution { get; set; }

        public string Remark { get; set; }
    }
}