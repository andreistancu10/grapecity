using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal sealed class UploadedFileRelationsFetcher : BaseRelationsFetcher
    {
        public IReadOnlyList<UserModel> UploadedFileUsers
            => GetItems<UploadedFilesUsersFetcher, UserModel>();

        public UploadedFileRelationsFetcher(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public UploadedFileRelationsFetcher UseUploadedFilesContext(UploadedFilesFetcherContext context)
        {
            Aggregator
                .UseRemoteFetcher<UploadedFilesUsersFetcher>(context);

            return this;
        }
    }
}