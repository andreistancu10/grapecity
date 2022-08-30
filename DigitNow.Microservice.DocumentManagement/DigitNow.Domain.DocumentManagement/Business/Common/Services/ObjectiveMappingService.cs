using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IObjectiveMappingService
    {
        Task<List<GeneralObjectiveViewModel>> MapToGeneralObjectiveViewModelAsync(IList<GeneralObjective> objectives, CancellationToken cancellationToken);

    }
    public class ObjectiveMappingService : IObjectiveMappingService
    {
        private readonly IMapper _mapper;

        private readonly GeneralObjectiveRelationsFetcher _generalObjectiveRelationsFetcher;
        public ObjectiveMappingService(IMapper mapper, IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _generalObjectiveRelationsFetcher = new GeneralObjectiveRelationsFetcher(serviceProvider);
        }
        public async Task<List<GeneralObjectiveViewModel>> MapToGeneralObjectiveViewModelAsync(IList<GeneralObjective> objectives, CancellationToken cancellationToken)
        {
            await _generalObjectiveRelationsFetcher
                .UseGeneralObjectivesContext(new GeneralObjectivesFetcherContext { GeneralObjectives = objectives })
                .TriggerFetchersAsync(cancellationToken);

            return MapGeneralObjectives(objectives)
                .ToList();
        }

        private List<GeneralObjectiveViewModel> MapGeneralObjectives(IEnumerable<GeneralObjective> generalObjectives)
        {
            var result = new List<GeneralObjectiveViewModel>();

            foreach (var generalObjective in generalObjectives)
            {
                var aggregate = new GeneralObjectiveAggregate
                {
                    GeneralObjective = generalObjective,
                    Users = _generalObjectiveRelationsFetcher.GeneralObjectiveUsers,
                    
                };

                result.Add(_mapper.Map<GeneralObjectiveAggregate, GeneralObjectiveViewModel>(aggregate));
            }

            return result;
        }
    }
}
