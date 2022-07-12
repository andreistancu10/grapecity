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
        public IList<Document> Documents
        {
            get => this[nameof(Documents)] as IList<Document>;
            set => this[nameof(Documents)] = value;
        }
    }
}
