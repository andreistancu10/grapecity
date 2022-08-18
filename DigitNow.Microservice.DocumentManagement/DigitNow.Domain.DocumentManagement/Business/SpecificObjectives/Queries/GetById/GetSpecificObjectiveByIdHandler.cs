using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using HTSS.Platform.Core.CQRS;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Queries.GetById
{
    public class GetSpecificObjectiveByIdHandler : IQueryHandler<GetSpecificObjectiveByIdQuery, GetSpecificObjectiveByIdResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISpecificObjectiveService _specificObjectiveService;

        public GetSpecificObjectiveByIdHandler(IMapper mapper,
            ISpecificObjectiveService specificObjectiveService)
        {
            _mapper = mapper;
            _specificObjectiveService = specificObjectiveService;
        }

        public async Task<GetSpecificObjectiveByIdResponse> Handle(GetSpecificObjectiveByIdQuery request, CancellationToken cancellationToken)
        {
            var specificObjective = _specificObjectiveService.FindQuery(item => item.ObjectiveId == request.ObjectiveId,
               new Expression<Func<SpecificObjective, object>>[] { item => item.Objective,
                   item => item.Objective.ObjectiveUploadedFiles,
                   item => item.AssociatedGeneralObjective.Objective,
                   item => item.SpecificObjectiveFunctionarys}).FirstOrDefault();

            if (specificObjective == null) return null;

            return _mapper.Map<GetSpecificObjectiveByIdResponse>(specificObjective);
        }
    }
}
