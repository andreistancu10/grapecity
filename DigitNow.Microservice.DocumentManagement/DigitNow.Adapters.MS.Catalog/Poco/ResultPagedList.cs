using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Adapters.MS.Catalog.Poco
{
    internal class ResultPagedList<T>
    {
        public IList<T> Items { get; set; }

        public PagingHeader PagingHeader { get; set; }
    }

    internal class PagingHeader
    {
        public int TotalItems { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }
    }
}
