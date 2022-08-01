using DigitNow.Domain.DocumentManagement.Data.Filters.Documents.Preprocess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Preprocess
{
    internal class ArchivedDocumentsFilterComponentContext : DataExpressionFilterComponentContext
    {
        public DocumentPreprocessFilter PreprocessFilter { get; set; }
    }
}
