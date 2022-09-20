using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Entities.Risks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IRiskMappingService
    {
        Task<List<RiskViewModel>> MapToRiskViewModelAsync(IList<Risk> risks, CancellationToken cancellationToken);
    }
    public class RiskMappingService : IRiskMappingService
    {
        private readonly IMapper _mapper;
        private readonly RiskRelationsFetcher _riskRelationsFetcher;

        public RiskMappingService(IMapper mapper, IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _riskRelationsFetcher = new RiskRelationsFetcher(serviceProvider);
        }
        public async Task<List<RiskViewModel>> MapToRiskViewModelAsync(IList<Risk> risks, CancellationToken cancellationToken)
        {
            await _riskRelationsFetcher
                .UseRiskFetcherContext(new RisksFetcherContext { Risks = risks})
                .TriggerFetchersAsync(cancellationToken);

            return MapRisks(risks).ToList();
        }

        private List<RiskViewModel> MapRisks(IList<Risk> risks)
        {
            var result = new List<RiskViewModel>();

            foreach (var risk in risks)
            {
                var aggregate = new RiskAggregate
                {
                    Risk = risk,
                    Departments = _riskRelationsFetcher.Departments,
                    SpecificObjectives = _riskRelationsFetcher.SpecificObjective
                };

                result.Add(_mapper.Map<RiskAggregate, RiskViewModel>(aggregate));
            }

            return result;
        }
    }
}
