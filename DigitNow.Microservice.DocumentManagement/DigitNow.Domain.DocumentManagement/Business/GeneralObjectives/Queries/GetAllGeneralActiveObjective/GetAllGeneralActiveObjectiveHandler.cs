using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Queries.GetAll
{
    internal class GetAllGeneralActiveObjectiveHandler : IQueryHandler<GetAllGeneralActiveObjectiveQuery, List<GetAllGeneralActiveObjectiveResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IGeneralObjectiveService _generalObjectiveService;

        public GetAllGeneralActiveObjectiveHandler(IMapper mapper,
            IGeneralObjectiveService generalObjectiveService)
        {
            _mapper = mapper;
            _generalObjectiveService = generalObjectiveService;
        }

        public async Task<List<GetAllGeneralActiveObjectiveResponse>> Handle(GetAllGeneralActiveObjectiveQuery request, CancellationToken cancellationToken)
        {
            var generalObjectives = await _generalObjectiveService.FindQuery()
                .Where(item => item.Objective.State != ObjectiveState.Inactive)
                .Include(item => item.Objective)
                .ToListAsync(cancellationToken);

            if (generalObjectives == null) return null;

            return _mapper.Map<List<GetAllGeneralActiveObjectiveResponse>>(generalObjectives);
        }
    }
}
