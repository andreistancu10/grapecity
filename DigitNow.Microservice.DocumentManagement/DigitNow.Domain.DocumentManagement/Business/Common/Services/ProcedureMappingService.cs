using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IProcedureMappingService
    {
        Task<List<ProcedureViewModel>> MapToProcedureViewModelAsync(IList<Procedure> procedures, CancellationToken cancellationToken);
    }
    public class ProcedureMappingService : IProcedureMappingService
    {
        private readonly IMapper _mapper; 
        private readonly ProcedureRelationsFetcher _procedureRelationsFetcher;

        public ProcedureMappingService(IMapper mapper, IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _procedureRelationsFetcher = new ProcedureRelationsFetcher(serviceProvider);
        }
        public async Task<List<ProcedureViewModel>> MapToProcedureViewModelAsync(IList<Procedure> procedures, CancellationToken cancellationToken)
        {
            await _procedureRelationsFetcher
               .UseProcedureFetcherContext(new ProceduresFetcherContext { Procedures = procedures })
               .TriggerFetchersAsync(cancellationToken);

            return MapProcedures(procedures).ToList();
        }
        private List<ProcedureViewModel> MapProcedures(IList<Procedure> procedures)
        {
            var result = new List<ProcedureViewModel>();

            foreach (var procedure in procedures)
            {
                var aggregate = new ProcedureAggregate
                {
                    Procedure = procedure,
                    Departments = _procedureRelationsFetcher.Departments,
                    SpecificObjectives = _procedureRelationsFetcher.SpecificObjectives,
                    Users = _procedureRelationsFetcher.Users
                };

                result.Add(_mapper.Map<ProcedureAggregate, ProcedureViewModel>(aggregate));
            }

            return result;
        }
    }
}
