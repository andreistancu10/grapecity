using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.ProcedureHistories.Queries.GetById
{
    public class GetProcedureHistoryByIdHandler : IQueryHandler<GetProcedureHistoryByIdQuery, ProcedureHistoryViewModel>
    {
        private readonly IProcedureHistoryService _procedureHistoryService;
        private readonly IProcedureHistoryMappingService _procedureHistoryMappingService;

        public GetProcedureHistoryByIdHandler(
            IProcedureHistoryService procedureHistoryService,
            IProcedureHistoryMappingService procedureHistoryMappingService)
        {
            _procedureHistoryService = procedureHistoryService;
            _procedureHistoryMappingService = procedureHistoryMappingService;
        }

        public async Task<ProcedureHistoryViewModel> Handle(GetProcedureHistoryByIdQuery request, CancellationToken cancellationToken)
        {
            var procedureHistory = await _procedureHistoryService.GetByIdQuery(request.Id)
                .Include(item => item.Procedure)
                .FirstOrDefaultAsync(cancellationToken);

            if (procedureHistory == null)
            {
                return new ProcedureHistoryViewModel();
            }

            var result = await _procedureHistoryMappingService.MapToProcedureHistoryViewModelAsync(
                new List<ProcedureHistory> { procedureHistory }, cancellationToken);

            return result.FirstOrDefault();
        }
    }
}
