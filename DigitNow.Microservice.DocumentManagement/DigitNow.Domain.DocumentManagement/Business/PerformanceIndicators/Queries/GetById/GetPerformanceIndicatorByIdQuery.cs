using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.PerformanceIndicators.Queries.GetById
{
    public class GetPerformanceIndicatorByIdQuery : IQuery<GetPerformanceIndicatorViewModel>
    {
        public long Id { get; set; }
        public GetPerformanceIndicatorByIdQuery(long id)
        {
            Id = id;
        }
    }
}
