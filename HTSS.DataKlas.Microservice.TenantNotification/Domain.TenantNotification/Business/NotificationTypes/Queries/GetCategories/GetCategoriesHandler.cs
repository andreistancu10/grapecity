using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HTSS.Platform.Core.CQRS;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Helpers;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Queries.GetCategories
{
    internal sealed class GetCategoriesHandler : IQueryHandler<GetCategoriesQuery, IList<GetCategoriesResponse>>
    {
        private readonly IMapper _mapper;

        public GetCategoriesHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task<IList<GetCategoriesResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = NotificationTypeCategoryHelper.GetNotificationTypeCategories();
            return Task.FromResult(_mapper.Map<IList<GetCategoriesResponse>>(categories));
        }
    }
}
