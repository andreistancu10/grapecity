using DigitNow.Domain.DocumentManagement.Business.Common.Models;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    internal class DocumentFileAggregate : VirtualFileAggregate
    {
        internal IReadOnlyList<DocumentCategoryModel> Categories { get; set; }
        internal IReadOnlyList<DocumentFileMappingModel> DocumentFileMappings { get; set; }
    }
}