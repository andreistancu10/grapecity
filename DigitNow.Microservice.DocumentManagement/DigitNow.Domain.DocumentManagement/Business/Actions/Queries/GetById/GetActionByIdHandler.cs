using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Actions.Queries.GetById
{
    public class GetActionByIdHandler : IQueryHandler<GetActionByIdQuery, GetActionByIdResponse>
    {
        private readonly IMapper _mapper;
        private readonly IActionService _actionService;
        private readonly ActivityRelationsFetcher _activityRelationsFetcher;

        public GetActionByIdHandler(
            IMapper mapper,
            IServiceProvider serviceProvider,
            IActionService actionService)
        {
            _mapper = mapper;
            _actionService = actionService;
            _activityRelationsFetcher = new ActivityRelationsFetcher(serviceProvider);
        }

        public async Task<GetActionByIdResponse> Handle(GetActionByIdQuery request, CancellationToken cancellationToken)
        {
            var action = await _actionService.FindQuery()
                .Include(c => c.AssociatedActivity)
                .AsNoTracking()
                .Include(x => x.ActionFunctionaries)
                .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

            if (action == null)
            {
                return null;
            }

            var associatedActivity = action.AssociatedActivity;

            await _activityRelationsFetcher.UseActivityFetcherContext(
                    new ActivitiesFetcherContext
                    {
                        Activities = new List<Activity>
                        {
                            associatedActivity
                        }
                    })
                .TriggerFetchersAsync(cancellationToken);

            var activityViewModel =
                _mapper.Map<ActivityAggregate, ActivityViewModel>(new ActivityAggregate
                {
                    Activity = associatedActivity,
                    Departments = _activityRelationsFetcher.Departments,
                    Users = _activityRelationsFetcher.Users,
                    GeneralObjectives = _activityRelationsFetcher.GeneralObjective,
                    SpecificObjectives = _activityRelationsFetcher.SpecificObjective
                });

            var response = _mapper.Map<GetActionByIdResponse>(action);
            response.Activity = activityViewModel;

            return response;
        }
    }
}
