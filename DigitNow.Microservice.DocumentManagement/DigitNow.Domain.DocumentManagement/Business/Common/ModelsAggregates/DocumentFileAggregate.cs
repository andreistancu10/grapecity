using DigitNow.Domain.DocumentManagement.Business.Common.Models;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    internal class DocumentFileAggregate 
    {
        internal DocumentFileModel DocumentFileModel { get; set; }
        internal IReadOnlyList<DocumentCategoryModel> Categories { get; set; }
        internal IReadOnlyList<DocumentFileMappingModel> DocumentFileMappings { get; set; }
        internal IReadOnlyList<UserModel> Users { get; set; }
    }
}