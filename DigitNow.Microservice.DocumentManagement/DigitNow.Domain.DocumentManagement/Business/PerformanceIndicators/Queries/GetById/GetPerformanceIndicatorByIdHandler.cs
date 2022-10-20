using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.PerformanceIndicators.Queries.GetById
{
    public class GetPerformanceIndicatorByIdHandler : IQueryHandler<GetPerformanceIndicatorByIdQuery, GetPerformanceIndicatorViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IPerformanceIndicatorService _performanceIndicatorService;
        public GetPerformanceIndicatorByIdHandler(IMapper mapper, IPerformanceIndicatorService performanceIndicatorService)
        {
            _mapper = mapper;
            _performanceIndicatorService = performanceIndicatorService; 
        }
        public async Task<GetPerformanceIndicatorViewModel> Handle(GetPerformanceIndicatorByIdQuery request, CancellationToken cancellationToken)
        {
            var performanceIndicator = await _performanceIndicatorService.GetByIdQuery(request.Id)
                .Include(item => item.AssociatedGeneralObjective.Objective)
                .Include(item => item.AssociatedSpecificObjective.Objective)
                .Include(item => item.AssociatedActivity.ActivityFunctionaries)
                .Include(item => item.PerformanceIndicatorFunctionaries)
                .FirstOrDefaultAsync(cancellationToken);

            if(performanceIndicator == null)
                return new GetPerformanceIndicatorViewModel();

            return _mapper.Map<GetPerformanceIndicatorViewModel>(performanceIndicator);

        }
    }
}
