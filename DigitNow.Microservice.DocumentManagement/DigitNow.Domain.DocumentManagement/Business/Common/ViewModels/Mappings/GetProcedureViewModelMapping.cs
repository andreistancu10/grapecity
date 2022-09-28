using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class GetProcedureViewModelMapping : Profile
    {
        public GetProcedureViewModelMapping()
        {
            CreateMap<Procedure, GetProcedureViewModel>()
                .ForPath(c => c.AssociatedGeneralObjective, opt => opt.MapFrom(src => src.AssociatedGeneralObjective))
                .ForPath(c => c.AssociatedGeneralObjective.Code, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.Code))
                .ForPath(c => c.AssociatedGeneralObjective.State, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.State))
                .ForPath(c => c.AssociatedGeneralObjective.Title, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.Title))
                .ForPath(c => c.AssociatedGeneralObjective.Details, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.Details))
                .ForPath(c => c.AssociatedGeneralObjective.ModificationMotive, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.ModificationMotive))
                .ForPath(c => c.AssociatedSpecificObjective, opt => opt.MapFrom(src => src.AssociatedSpecificObjective))
                .ForPath(c => c.AssociatedSpecificObjective.Code, opt => opt.MapFrom(src => src.AssociatedSpecificObjective.Objective.Code))
                .ForPath(c => c.AssociatedSpecificObjective.State, opt => opt.MapFrom(src => src.AssociatedSpecificObjective.Objective.State))
                .ForPath(c => c.AssociatedSpecificObjective.Title, opt => opt.MapFrom(src => src.AssociatedSpecificObjective.Objective.Title))
                .ForPath(c => c.AssociatedSpecificObjective.Details, opt => opt.MapFrom(src => src.AssociatedSpecificObjective.Objective.Details))
                .ForPath(c => c.AssociatedSpecificObjective.ModificationMotive, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.ModificationMotive))
                .ForPath(c => c.AssociatedActivity, opt => opt.MapFrom(src => src.AssociatedActivity))
                .ForPath(c => c.FunctionaryIds, opt => opt.MapFrom(src => src.ProcedureFunctionaries.Select(x => x.FunctionaryId).ToList()));
        }
    }
}
