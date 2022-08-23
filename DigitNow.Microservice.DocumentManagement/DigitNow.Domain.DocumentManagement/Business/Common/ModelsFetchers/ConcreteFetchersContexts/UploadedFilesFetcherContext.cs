using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts
{
    internal class UploadedFilesFetcherContext : ModelFetcherContext
    {
        public IList<UploadedFile> UploadedFiles
        {
            get => this[nameof(UploadedFiles)] as IList<UploadedFile>;
            set => this[nameof(UploadedFiles)] = value;
        }
    }
}