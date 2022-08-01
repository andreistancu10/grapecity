using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Documents.Postprocess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Documents.Postprocess
{
    internal class ActiveDocumentsPostprocessFilterComponentContext : DataExpressionFilterComponentContext
    {
        public DocumentPostprocessFilter PostprocessFilter { get; set; }
    }
}
