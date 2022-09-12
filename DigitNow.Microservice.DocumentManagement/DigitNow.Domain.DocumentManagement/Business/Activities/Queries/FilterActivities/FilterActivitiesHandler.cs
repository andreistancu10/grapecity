using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Activities;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
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
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly ActivityRelationsFetcher _activityRelationsFetcher;

        public FilterActivitiesHandler(
            IActivityService activityService,
            IMapper mapper,
            IServiceProvider serviceProvider,
            IIdentityService identityService)
        {
            _activityService = activityService;
            _mapper = mapper;
            _identityService = identityService;
            _activityRelationsFetcher = new ActivityRelationsFetcher(serviceProvider);
        }

        public async Task<ResultPagedList<ActivityViewModel>> Handle(FilterActivitiesQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _identityService.GetCurrentUserAsync(cancellationToken);
            request.DepartmentIds = currentUser.Departments.Select(c => c.Id);
            var activitiesPagedList = await _activityService.GetActivitiesAsync(_mapper.Map<ActivityFilter>(request), cancellationToken);

            if (activitiesPagedList.List.Count == 0)
            {
                return new ResultPagedList<ActivityViewModel>(new PagingHeader(0, 0, 0, 0), null);
            }

            await _activityRelationsFetcher.UseActivityFetcherContext(
                    new ActivitiesFetcherContext
                    {
                        Activities = activitiesPagedList.List
                    })
                .TriggerFetchersAsync(cancellationToken);

            var activityViewModels = activitiesPagedList.List.Select(c =>
                _mapper.Map<ActivityAggregate, ActivityViewModel>(new ActivityAggregate
                {
                    Activity = c,
                    Departments = _activityRelationsFetcher.Departments,
                    Users = _activityRelationsFetcher.Users,
                    GeneralObjectives = _activityRelationsFetcher.GeneralObjective,
                    SpecificObjectives = _activityRelationsFetcher.SpecificObjective
                }))
                .ToList();

            var resultPagedList = new ResultPagedList<ActivityViewModel>(activitiesPagedList.GetHeader(), activityViewModels.OrderByDescending(x => x.CreatedAt).ToList());

            return resultPagedList;
        }
    }
}
