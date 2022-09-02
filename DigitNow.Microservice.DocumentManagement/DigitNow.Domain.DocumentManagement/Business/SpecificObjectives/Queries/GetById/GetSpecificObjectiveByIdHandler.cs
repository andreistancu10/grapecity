using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Queries.GetById
{
    public class GetSpecificObjectiveByIdHandler : IQueryHandler<GetSpecificObjectiveByIdQuery, GetSpecificObjectiveViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ISpecificObjectiveService _specificObjectiveService;

        public GetSpecificObjectiveByIdHandler(IMapper mapper,
            ISpecificObjectiveService specificObjectiveService)
        {
            _mapper = mapper;
            _specificObjectiveService = specificObjectiveService;

        }

        public async Task<GetSpecificObjectiveViewModel> Handle(GetSpecificObjectiveByIdQuery request, CancellationToken cancellationToken)
        {
            var specificObjective = await _specificObjectiveService.FindQuery()
                .Where(item => item.ObjectiveId == request.ObjectiveId)
                .Include(item => item.Objective)
                .Include(item => item.AssociatedGeneralObjective.Objective)
                .Include(item => item.SpecificObjectiveFunctionarys).FirstOrDefaultAsync(cancellationToken);

            if (specificObjective == null)
            {
                return null;
            }

            return _mapper.Map<GetSpecificObjectiveViewModel>(specificObjective);
        }
    }
}
