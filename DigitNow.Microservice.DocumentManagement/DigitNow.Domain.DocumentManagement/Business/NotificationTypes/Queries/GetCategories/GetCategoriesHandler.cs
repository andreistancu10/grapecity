using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Helpers;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Queries.GetCategories
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
