using DigitNow.Domain.DocumentManagement.Data.Filters.PublicAcquisitions;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.PublicAcquisitions.Queries.GetPublicAcquisitions
{
    public class GetPublicAcquisitionsQuery : IQuery<ResultObject>
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public PublicAcquisitionFilter Filter { get; set; }
    }
}
