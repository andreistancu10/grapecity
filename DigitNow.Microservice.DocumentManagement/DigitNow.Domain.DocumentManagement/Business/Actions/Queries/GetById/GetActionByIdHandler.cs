﻿using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Activities.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Actions.Queries.GetById
{
    public class GetActionByIdHandler : IQueryHandler<GetActionByIdQuery, GetActionByIdResponse>
    {
        private readonly IMapper _mapper;
        private readonly IActionService _actionService;
        private readonly IServiceProvider _serviceProvider;

        public GetActionByIdHandler(
            IMapper mapper,
            IServiceProvider serviceProvider,
            IActionService actionService)
        {
            _mapper = mapper;
            _serviceProvider = serviceProvider;
            _actionService = actionService;
        }

        public async Task<GetActionByIdResponse> Handle(GetActionByIdQuery request, CancellationToken cancellationToken)
        {
            var action = await _actionService.FindQuery()
                .AsNoTracking()
                .Include(x => x.ActionFunctionaries)
                .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

            if (action == null)
            {
                return null;
            }

            var getActivityByIdHandler = new GetActivityByIdHandler(_mapper, _serviceProvider.GetService<IActivityService>());
            var activityViewModel = await getActivityByIdHandler.Handle(new GetActivityByIdQuery(action.ActivityId), cancellationToken);
            var response = _mapper.Map<GetActionByIdResponse>(action);
            response.Activity = activityViewModel;

            return response;
        }
    }
}
