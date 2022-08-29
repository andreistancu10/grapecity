using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public class GeneralObjectiveViewModelMapping : Profile
    {
        public GeneralObjectiveViewModelMapping()
        {
            CreateMap<GeneralObjective, BasicGeneralObjectiveViewModel>()
                .ForMember(m => m.CreatedDate, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(m => m.UpdatedDate, opt => opt.MapFrom(src => src.ModifiedAt))
                .ForMember(m => m.Code, opt => opt.MapFrom(src => src.Objective.Code))
                .ForMember(m => m.Title, opt => opt.MapFrom(src => src.Objective.Title))
                .ForMember(m => m.State, opt => opt.MapFrom(src => src.Objective.State));
        }
    }

    public interface IObjectiveMappingService
    {
        Task<List<BasicGeneralObjectiveViewModel>> MapToGeneralObjectiveViewModelAsync(IList<GeneralObjective> objectives, CancellationToken cancellationToken);

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
        public async Task<List<BasicGeneralObjectiveViewModel>> MapToGeneralObjectiveViewModelAsync(IList<GeneralObjective> objectives, CancellationToken cancellationToken)
        {
            await _generalObjectiveRelationsFetcher
                .UseGeneralObjectivesContext(new GeneralObjectivesFetcherContext { GeneralObjectives = objectives })
                .TriggerFetchersAsync(cancellationToken);

            var result = new List<BasicGeneralObjectiveViewModel>();

            foreach (var objective in objectives)
            {
                var generalObjectiveViewModel = _mapper.Map<GeneralObjective, BasicGeneralObjectiveViewModel>(objective);
                
                var createdByUser = _generalObjectiveRelationsFetcher.GeneralObjectiveUsers.FirstOrDefault(x => x.Id == objective.CreatedBy);

                if (createdByUser != null)
                {
                    generalObjectiveViewModel.CreatedBy = $"{createdByUser.FirstName} {createdByUser.LastName}"; 
                }

                result.Add(generalObjectiveViewModel);
            }

            return result;
        }
    }
}
