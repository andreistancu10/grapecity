using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public class GeneralObjectiveViewModelMapping : Profile
    {
        public GeneralObjectiveViewModelMapping()
        {
            CreateMap<Objective, BasicGeneralObjectiveViewModel>()
                .ForMember(m => m.CreatedDate, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(m => m.UpdatedDate, opt => opt.MapFrom(src => src.ModifiedAt));
        }
    }

    public interface IObjectiveMappingService
    {
        Task<List<BasicGeneralObjectiveViewModel>> MapToGeneralObjectiveViewModelAsync(IList<Objective> objectives, CancellationToken cancellationToken);

    }
    public class ObjectiveMappingService : IObjectiveMappingService
    {
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        public ObjectiveMappingService(IMapper mapper, IIdentityService identityService)
        {
            _mapper = mapper;
            _identityService = identityService;
        }
        public Task<List<BasicGeneralObjectiveViewModel>> MapToGeneralObjectiveViewModelAsync(IList<Objective> objectives, CancellationToken cancellationToken)
        {
            return MapObjectives(objectives, cancellationToken);
        }

        private async Task<List<BasicGeneralObjectiveViewModel>> MapObjectives(IEnumerable<Objective> objectives, CancellationToken cancellationToken)
        {
            var result = new List<BasicGeneralObjectiveViewModel>();

            foreach (var objective in objectives)
            {

                var generalObjectiveViewModel = _mapper.Map<Objective, BasicGeneralObjectiveViewModel>(objective);
                var createdByUser = await _identityService.GetUserByIdAsync(objective.CreatedBy, cancellationToken);
                if(createdByUser != null)
                {
                    generalObjectiveViewModel.CreatedBy = $"{createdByUser.FirstName} {createdByUser.LastName}"; 
                }

                result.Add(generalObjectiveViewModel);
            }

            return result;
        }

    }
}
