using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Filters.Activities;
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
            var page = request.PageNumber ?? 1;
            var count = request.PageSize ?? 50;

            var activities = await _activityService.GetActivitiesAsync(request.Filter, currentUser, page, count, cancellationToken);

            if (activities.Count == 0)
            {
                return new ResultPagedList<ActivityViewModel>(new PagingHeader(0, 0, 0, 0), null);
            }

            await _activityRelationsFetcher.UseActivityFetcherContext(
                    new ActivitiesFetcherContext
                    {
                        Activities = activities
                    })
                .TriggerFetchersAsync(cancellationToken);

            var activityViewModels = activities.Select(c =>
                _mapper.Map<ActivityAggregate, ActivityViewModel>(new ActivityAggregate
                {
                    Activity = c,
                    Departments = _activityRelationsFetcher.Departments,
                    Users = _activityRelationsFetcher.Users,
                    GeneralObjectives = _activityRelationsFetcher.GeneralObjective,
                    SpecificObjectives = _activityRelationsFetcher.SpecificObjective
                }))
                .ToList();

            var pagedList = new PagedList<ActivityViewModel>(activityViewModels, activityViewModels.Count, page, count);
            var resultPagedList = new ResultPagedList<ActivityViewModel>(pagedList.GetHeader(), activityViewModels.OrderByDescending(x => x.CreatedAt).ToList());

            return resultPagedList;
        }
    }
}
