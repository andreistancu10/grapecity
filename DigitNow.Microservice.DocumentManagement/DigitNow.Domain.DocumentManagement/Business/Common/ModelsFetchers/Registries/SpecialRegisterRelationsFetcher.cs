using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal sealed class SpecialRegisterRelationsFetcher : BaseRelationsFetcher
    {
        public IReadOnlyList<DocumentCategoryModel> DocumentCategories
            => GetItems<GenericDocumentsCategoriesFetcher, DocumentCategoryModel>();


        public SpecialRegisterRelationsFetcher(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            Aggregator
                .UseGenericRemoteFetcher<GenericDocumentsCategoriesFetcher>();
        }
    }
}