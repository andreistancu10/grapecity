﻿using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Activities.Queries.GetById
{
    public class GetActivityByIdHandler : IQueryHandler<GetActivityByIdQuery, GetActivityViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IActivityService _activityService;

        public GetActivityByIdHandler(IMapper mapper,
            IActivityService activityService)
        {
            _mapper = mapper;
            _activityService = activityService;
        }

        public async Task<GetActivityViewModel> Handle(GetActivityByIdQuery request, CancellationToken cancellationToken)
        {
            var activity = await _activityService.FindQuery()
                .Where(item => item.Id == request.Id)
                .Include(item => item.AssociatedGeneralObjective)
                .Include(item => item.AssociatedSpecificObjective.Objective)
                .Include(item => item.ActivityFunctionarys).FirstOrDefaultAsync(cancellationToken);

            if (activity == null)
                return null;

            return _mapper.Map<GetActivityViewModel>(activity);
        }
    }
}