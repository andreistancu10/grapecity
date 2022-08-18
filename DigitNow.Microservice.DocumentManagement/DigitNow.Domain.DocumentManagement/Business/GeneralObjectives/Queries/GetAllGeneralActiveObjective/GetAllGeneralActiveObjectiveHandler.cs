using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using HTSS.Platform.Core.CQRS;

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
            var generalObjectives = _generalObjectiveService.FindAllQuery(item => item.Objective.State != ObjectiveState.Inactive).ToList();

            if (generalObjectives == null) return null;

            return _mapper.Map<List<GetAllGeneralActiveObjectiveResponse>>(generalObjectives);
        }
    }
}
