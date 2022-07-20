using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts
{
    internal class DocumentsFetcherContext : ModelFetcherContext
    {
        public IList<VirtualDocument> Documents
        {
            get => this[nameof(Documents)] as IList<VirtualDocument>;
            set => this[nameof(Documents)] = value;
        }
    }
}
