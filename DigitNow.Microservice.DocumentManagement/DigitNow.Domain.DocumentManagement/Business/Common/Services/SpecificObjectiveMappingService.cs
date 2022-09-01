using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface ISpecificObjectiveMappingService
    {
        Task<List<SpecificObjectiveViewModel>> MapToSpecificObjectiveViewModelAsync(IList<SpecificObjective> specificObjectives, CancellationToken cancellationToken);

    }
    public class SpecificObjectiveMappingService : ISpecificObjectiveMappingService
    {
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        private UserModel _currentUser;
        private readonly SpecificObjectiveRelationsFetcher _specificObjectiveRelationsFetcher;

        public SpecificObjectiveMappingService(
            IServiceProvider serviceProvider,
            IIdentityService identityService,
            IMapper mapper)
        {
            _identityService = identityService;
            _mapper = mapper;
            _specificObjectiveRelationsFetcher = new SpecificObjectiveRelationsFetcher(serviceProvider);
        }

        public async Task<List<SpecificObjectiveViewModel>> MapToSpecificObjectiveViewModelAsync(IList<SpecificObjective> specificObjectives, CancellationToken cancellationToken)
        {
            if (!specificObjectives.Any())
            {
                return new List<SpecificObjectiveViewModel>();
            }

            _currentUser = await _identityService.GetCurrentUserAsync(cancellationToken);

            await _specificObjectiveRelationsFetcher
                .UseSpecificObjectiveContext(new ObjectivesFetcherContext { Objectives = specificObjectives })
                .TriggerFetchersAsync(cancellationToken);

            return MapSpecificObjectivess(specificObjectives)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }

        private List<SpecificObjectiveViewModel> MapSpecificObjectivess(IList<SpecificObjective> specificObjectives)
        {
            var result = new List<SpecificObjectiveViewModel>();


            if (specificObjectives.Any())
            {
                result.AddRange(MapChildDocuments(specificObjectives));
            }
            return result;
        }

        private List<SpecificObjectiveViewModel> MapChildDocuments<T>(IEnumerable<T> childObjectives)
           where T : VirtualObjective
        {
            var result = new List<SpecificObjectiveViewModel>();

            foreach (var childObjective in childObjectives)
            {
                var aggregate = new VirtualObjectiveAgregate<T>
                {
                    VirtualObjective = childObjective,
                    CurrentUser = _currentUser,
                    Users = _specificObjectiveRelationsFetcher.ObjectiveUsers,
                    Departments = _specificObjectiveRelationsFetcher.ObjectiveDepartments
                };

                result.Add(_mapper.Map<VirtualObjectiveAgregate<T>, SpecificObjectiveViewModel>(aggregate));
            }
            return result;
        }
    }
}
