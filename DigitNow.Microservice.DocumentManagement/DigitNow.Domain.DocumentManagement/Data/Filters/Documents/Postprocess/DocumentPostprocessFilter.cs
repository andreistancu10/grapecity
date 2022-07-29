using DigitNow.Domain.DocumentManagement.Data.Filters.Documents.Preprocess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Documents.Postprocess
{
    public class DocumentPostprocessFilter : DataFilter
    {
        public DocumentCategoryFilter CategoryFilter { get; set; }

        public static DocumentPostprocessFilter Empty => new DocumentPostprocessFilter();
    }

    public class DocumentCategoryFilter
    {
        public List<long> CategoryIds { get; set; }
    }
}
