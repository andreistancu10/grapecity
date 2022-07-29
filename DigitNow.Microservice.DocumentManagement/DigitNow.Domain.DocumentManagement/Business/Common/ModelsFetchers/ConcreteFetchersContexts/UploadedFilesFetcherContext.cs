using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.UploadedFiles;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts
{
    internal class UploadedFilesFetcherContext : ModelFetcherContext
    {
        public IList<UploadedFile> UploadFiles 
        {
            get => this[nameof(UploadFiles)] as IList<UploadedFile>;
            set => this[nameof(UploadFiles)] = value;
        }
    }
}