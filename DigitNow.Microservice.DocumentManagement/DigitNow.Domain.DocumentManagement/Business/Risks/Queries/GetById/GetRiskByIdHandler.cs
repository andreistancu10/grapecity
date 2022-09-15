using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Risks.Queries.GetById
{
    public class GetRiskByIdHandler : IQueryHandler<GetRiskByIdQuery, GetRiskViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IRiskService _riskService;

        public GetRiskByIdHandler(IMapper mapper,
            IRiskService riskService)
        {
            _mapper = mapper;
            _riskService = riskService;
        }

        public async Task<GetRiskViewModel> Handle(GetRiskByIdQuery request, CancellationToken cancellationToken)
        {
            var risk = await _riskService.FindQuery()
                .Where(item => item.Id == request.Id)
                .Include(item => item.AssociatedGeneralObjective)
                .Include(item => item.AssociatedGeneralObjective.Objective)
                .Include(item => item.AssociatedSpecificObjective)
                .Include(item => item.AssociatedSpecificObjective.Objective)
                .Include(item => item.AssociatedActivity)
                .Include(item => item.AssociatedAction)
                .Include(item => item.RiskControlActions)
                .FirstOrDefaultAsync(cancellationToken);

            if (risk == null)
                return null;

            return _mapper.Map<GetRiskViewModel>(risk);
        }
    }
}
