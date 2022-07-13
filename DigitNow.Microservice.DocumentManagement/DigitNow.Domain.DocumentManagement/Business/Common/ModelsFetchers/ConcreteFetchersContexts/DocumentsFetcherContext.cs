using DigitNow.Domain.DocumentManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers
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
