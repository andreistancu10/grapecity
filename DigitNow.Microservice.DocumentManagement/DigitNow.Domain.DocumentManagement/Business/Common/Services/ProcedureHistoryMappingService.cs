using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IProcedureHistoryMappingService
    {
        Task<List<ProcedureHistoryViewModel>> MapToProcedureHistoryViewModelAsync(IList<ProcedureHistory> procedureHistories, CancellationToken cancellationToken);
    }

    public class ProcedureHistoryMappingService : IProcedureHistoryMappingService
    {
        private readonly IMapper _mapper; 
        private readonly ProcedureHistoryRelationsFetcher _procedureHistoryRelationsFetcher;

        public ProcedureHistoryMappingService(IMapper mapper, IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _procedureHistoryRelationsFetcher = new ProcedureHistoryRelationsFetcher(serviceProvider);
        }

        public async Task<List<ProcedureHistoryViewModel>> MapToProcedureHistoryViewModelAsync(IList<ProcedureHistory> procedureHistories, CancellationToken cancellationToken)
        {
            await _procedureHistoryRelationsFetcher
               .UseProcedureHistoryFetcherContext(new ProcedureHistoriesFetcherContext { ProcedureHistories = procedureHistories })
               .TriggerFetchersAsync(cancellationToken);

            return MapProcedureHistories(procedureHistories).ToList();
        }

        private IEnumerable<ProcedureHistoryViewModel> MapProcedureHistories(IList<ProcedureHistory> procedureHistories)
        {
            var result = new List<ProcedureHistoryViewModel>();

            foreach (var procedureHistory in procedureHistories)
            {
                var aggregate = new ProcedureHistoryAggregate
                {
                    ProcedureHistory = procedureHistory,
                    Users = _procedureHistoryRelationsFetcher.Users,
                    Procedure = procedureHistory.Procedure
                };

                result.Add(_mapper.Map<ProcedureHistoryAggregate, ProcedureHistoryViewModel>(aggregate));
            }

            return result;
        }
    }
}
