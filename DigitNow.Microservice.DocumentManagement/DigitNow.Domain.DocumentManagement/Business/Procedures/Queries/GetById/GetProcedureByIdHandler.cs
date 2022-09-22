using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Procedures.Queries.GetById
{
    public class GetProcedureByIdHandler : IQueryHandler<GetProcedureByIdQuery, GetProcedureViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IProcedureService _procedureService;

        public GetProcedureByIdHandler(IMapper mapper,
            IProcedureService procedureService)
        {
            _mapper = mapper;
            _procedureService = procedureService;
        }

        public async Task<GetProcedureViewModel> Handle(GetProcedureByIdQuery request, CancellationToken cancellationToken)
        {
            var procedure = await _procedureService.FindQuery()
                .Where(item => item.Id == request.Id)
                .Include(item => item.AssociatedGeneralObjective)
                .Include(item => item.AssociatedGeneralObjective.Objective)
                .Include(item => item.AssociatedSpecificObjective)
                .Include(item => item.AssociatedSpecificObjective.Objective)
                .Include(item => item.AssociatedSpecificObjective.SpecificObjectiveFunctionaries)
                .Include(item => item.AssociatedActivity)
                .Include(item => item.AssociatedActivity.ActivityFunctionaries)
                .Include(item => item.ProcedureFunctionaries)
                .FirstOrDefaultAsync(cancellationToken);

            if (procedure == null)
                return new GetProcedureViewModel();

            return _mapper.Map<GetProcedureViewModel>(procedure);
        }
    }
}
