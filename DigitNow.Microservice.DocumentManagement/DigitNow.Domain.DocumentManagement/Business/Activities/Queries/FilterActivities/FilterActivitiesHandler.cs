using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.Activities.Queries.FilterActivities
{
    public class FilterActivitiesHandler : IQueryHandler<FilterActivitiesQuery, ResultPagedList<ActivityViewModel>>
    {
        private readonly IActivityService _activityService;
        private readonly IMapper _mapper;

        public FilterActivitiesHandler(
            IActivityService activityService,
            IMapper mapper)
        {
            _activityService = activityService;
            _mapper = mapper;
        }

        public async Task<ResultPagedList<ActivityViewModel>> Handle(FilterActivitiesQuery request, CancellationToken cancellationToken)
        {
            var totalItems = await _activityService.CountActivitiesAsync(request.Filter, cancellationToken);

            var pagingHeader = new PagingHeader(
                (int)totalItems,
                request.Page,
                request.Count,
                (int)Math.Ceiling((decimal)totalItems / request.Count));

            if (totalItems == 0)
            {
                return new ResultPagedList<ActivityViewModel>(pagingHeader, null);
            }

            var activities = await _activityService.GetActivitiesAsync(request.Filter,
                request.Page,
                request.Count,
                cancellationToken);

            var viewModels = _mapper.Map<List<Activity>, List<ActivityViewModel>>(activities);
            var resultPage = new ResultPagedList<ActivityViewModel>(pagingHeader, viewModels);

            return resultPage;
        }
    }
}
