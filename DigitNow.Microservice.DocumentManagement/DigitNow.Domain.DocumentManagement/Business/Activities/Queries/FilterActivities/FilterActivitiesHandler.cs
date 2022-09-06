using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.Activities.Queries.FilterActivities
{
    public class FilterActivitiesHandler : IQueryHandler<FilterActivitiesQuery, ResultPagedList<ActivityViewModel>>
    {
        private readonly IActivityService _activityService;
        private readonly IMapper _mapper;
        private readonly ActivityRelationsFetcher _activityRelationsFetcher;

        public FilterActivitiesHandler(
            IActivityService activityService,
            IMapper mapper,
            IServiceProvider serviceProvider)
        {
            _activityService = activityService;
            _mapper = mapper;
            _activityRelationsFetcher = new ActivityRelationsFetcher(serviceProvider);
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

            await _activityRelationsFetcher.TriggerFetchersAsync(cancellationToken);

            var result = activities.Select(c =>
                _mapper.Map<ActivityAggregate, ActivityViewModel>(new ActivityAggregate
                {
                    Activity = c,
                    Departments = _activityRelationsFetcher.Departments,
                    Users = _activityRelationsFetcher.Users
                }))
                .ToList();

            var resultPage = new ResultPagedList<ActivityViewModel>(pagingHeader, result);

            return resultPage;
        }
    }
}
