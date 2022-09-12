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

namespace DigitNow.Domain.DocumentManagement.Business.Actions.Queries.FilterActions
{
    public class FilterActionsHandler : IQueryHandler<FilterActionsQuery, ResultPagedList<ActionViewModel>>
    {
        private readonly IActionService _actionService;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly ActionRelationsFetcher _actionRelationsFetcher;

        public FilterActionsHandler(
            IActionService actionService,
            IMapper mapper,
            IServiceProvider serviceProvider,
            IIdentityService identityService)
        {
            _actionService = actionService;
            _mapper = mapper;
            _identityService = identityService;
            _actionRelationsFetcher = new ActionRelationsFetcher(serviceProvider);
        }

        public async Task<ResultPagedList<ActionViewModel>> Handle(FilterActionsQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _identityService.GetCurrentUserAsync(cancellationToken);
            var actionsPagedList = await _actionService.GetActionsAsync(_mapper.Map<ActionFilter>(request), cancellationToken);

            if (actionsPagedList.List.Count == 0)
            {
                return new ResultPagedList<ActionViewModel>(new PagingHeader(0, 0, 0, 0), new List<ActionViewModel>());
            }

            await _actionRelationsFetcher.UseActionFetcherContext(
                    new ActionsFetcherContext
                    {
                        Actions = actionsPagedList.List
                    })
                .TriggerFetchersAsync(cancellationToken);

            var actionViewModels = actionsPagedList.List.Select(c =>
                _mapper.Map<ActionAggregate, ActionViewModel>(new ActionAggregate
                {
                    Action = c,
                    Departments = _actionRelationsFetcher.Departments,
                    Users = _actionRelationsFetcher.Users,
                    GeneralObjectives = _actionRelationsFetcher.GeneralObjective,
                    SpecificObjectives = _actionRelationsFetcher.SpecificObjective
                }))
                .ToList();

            var resultPagedList = new ResultPagedList<ActionViewModel>(actionsPagedList.GetHeader(), actionViewModels.OrderByDescending(x => x.CreatedAt).ToList());

            return resultPagedList;
        }
    }
}
