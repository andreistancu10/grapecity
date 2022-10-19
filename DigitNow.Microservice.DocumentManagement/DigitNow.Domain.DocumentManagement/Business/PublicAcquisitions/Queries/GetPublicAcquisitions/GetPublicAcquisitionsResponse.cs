using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;

namespace DigitNow.Domain.DocumentManagement.Business.PublicAcquisitions.Queries.GetPublicAcquisitions
{
    internal class GetPublicAcquisitionsResponse
    {
        public long TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalPages { get; set; }
        public IList<PublicAcquisitionViewModel> Items { get; set; }
    }
}
