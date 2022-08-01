using DigitNow.Domain.DocumentManagement.Data.Filters.Documents.Postprocess;
using DigitNow.Domain.DocumentManagement.Data.Filters.Documents.Preprocess;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Documents
{
    public class DocumentFilter
    {
        public DocumentPreprocessFilter PreprocessFilter { get; set; }
        public bool HasPreprocessFilter => PreprocessFilter != null;

        public DocumentPostprocessFilter PostProcessFilter { get; set; }
        public bool HasPostprocessFilter => PostProcessFilter != null;

        public bool IsEmpty() => PreprocessFilter == null && PostProcessFilter == null;

        public static DocumentFilter Empty => new DocumentFilter
        {
            PreprocessFilter = DocumentPreprocessFilter.Empty,
            PostProcessFilter = DocumentPostprocessFilter.Empty
        };
    }
}
