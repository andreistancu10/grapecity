using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries
{
    public class GetDocumentsResponse
    {
        public long TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalPages { get; set; }
        public IList<DocumentViewModel> Items { get; set; }
    }
}