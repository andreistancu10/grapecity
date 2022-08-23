using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Queries.GetById
{
    public class GetGeneralObjectiveByIdHandler : IQueryHandler<GetGeneralObjectiveByIdQuery, GetGeneralObjectiveByIdResponse>
    {
        private readonly IMapper _mapper;
        private readonly IGeneralObjectiveService _generalObjectiveService;

        public GetGeneralObjectiveByIdHandler(IMapper mapper,
            IGeneralObjectiveService generalObjectiveService)
        {
            _mapper = mapper;
            _generalObjectiveService = generalObjectiveService;
        }

        public async Task<GetGeneralObjectiveByIdResponse> Handle(GetGeneralObjectiveByIdQuery request, CancellationToken cancellationToken)
        {
            var generalObjective = await _generalObjectiveService.FindQuery()
                .Where(item => item.ObjectiveId == request.ObjectiveId)
                .Include(item => item.Objective)
                .ThenInclude(item => item.ObjectiveUploadedFiles )
                .FirstOrDefaultAsync(cancellationToken);

            if (generalObjective == null)
            {
                return null;
            }



            return _mapper.Map<GetGeneralObjectiveByIdResponse>(generalObjective);
        }
    }
}
