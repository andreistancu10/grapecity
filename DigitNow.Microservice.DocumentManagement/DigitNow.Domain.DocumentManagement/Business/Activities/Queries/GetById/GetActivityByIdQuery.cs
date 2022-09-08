using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Activities.Queries.GetById
{
    public class GetActivityByIdQuery : IQuery<GetActivityViewModel>
    {
        public long Id { get; set; }
        public GetActivityByIdQuery(long id)
        {
            Id = id;
        }
    }
}
