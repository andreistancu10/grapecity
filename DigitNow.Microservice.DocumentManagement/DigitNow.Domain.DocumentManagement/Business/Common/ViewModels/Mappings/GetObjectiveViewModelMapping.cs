using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class GetObjectiveViewModelMapping : Profile
    {
        public GetObjectiveViewModelMapping()
        {
            CreateMap<SpecificObjective, GetSpecificObjectiveViewModel>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.ObjectiveId))
                .ForMember(c => c.Code, opt => opt.MapFrom(src => src.Objective.Code))
                .ForMember(c => c.StateId, opt => opt.MapFrom(src => src.Objective.StateId))
                .ForMember(c => c.Title, opt => opt.MapFrom(src => src.Objective.Title))
                .ForMember(c => c.Details, opt => opt.MapFrom(src => src.Objective.Details))
                .ForMember(c => c.ModificationMotive, opt => opt.MapFrom(src => src.Objective.ModificationMotive))
                .ForMember(c => c.GeneralObjectiveId, opt => opt.MapFrom(src => src.GeneralObjectiveId))
                .ForMember(c => c.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
                .ForPath(c => c.AssociatedGeneralObjective.ObjectiveId, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.ObjectiveId))
                .ForPath(c => c.AssociatedGeneralObjective.Code, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.Code))
                .ForPath(c => c.AssociatedGeneralObjective.StateId, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.StateId))
                .ForPath(c => c.AssociatedGeneralObjective.Title, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.Title))
                .ForPath(c => c.AssociatedGeneralObjective.Details, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.Details))
                .ForPath(c => c.AssociatedGeneralObjective.ModificationMotive, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.ModificationMotive))
                .ForPath(c => c.FunctionaryId, opt => opt.MapFrom(src => src.SpecificObjectiveFunctionaries.Select(x => x.FunctionaryId).ToList()));

            CreateMap<GeneralObjectiveAggregate, GeneralObjectiveViewModel>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.GeneralObjective.ObjectiveId))
                .ForMember(c => c.Code, opt => opt.MapFrom(src => src.GeneralObjective.Objective.Code))
                .ForMember(c => c.StateId, opt => opt.MapFrom(src => src.GeneralObjective.Objective.StateId))
                .ForMember(c => c.Title, opt => opt.MapFrom(src => src.GeneralObjective.Objective.Title))
                .ForMember(c => c.Details, opt => opt.MapFrom(src => src.GeneralObjective.Objective.Details))
                .ForMember(c => c.ModificationMotive, opt => opt.MapFrom(src => src.GeneralObjective.Objective.ModificationMotive))
                .ForMember(c => c.CreatedAt, opt => opt.MapFrom(src => src.GeneralObjective.CreatedAt))
                .ForMember(c => c.ModifiedAt, opt => opt.MapFrom(src => src.GeneralObjective.Objective.ModifiedAt ?? src.GeneralObjective.Objective.CreatedAt))
                .ForMember(c => c.CreatedBy, opt => opt.MapFrom<MapObjectiveUser>())
                .ForMember(c => c.ModifiedBy, opt => opt.MapFrom<MapModifiedBy>());
        }

        private class MapObjectiveUser : IValueResolver<GeneralObjectiveAggregate, GeneralObjectiveViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(GeneralObjectiveAggregate source, GeneralObjectiveViewModel destination, BasicViewModel destMember, ResolutionContext context) =>
                ExtractUser(source);

            private static BasicViewModel ExtractUser(GeneralObjectiveAggregate source)
            {
                var foundUser = source.Users.FirstOrDefault(x => x.Id == source.GeneralObjective.CreatedBy);
                if(foundUser != null)
                {
                    return new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
                }
                return default;
            }
        }

        private class MapModifiedBy : IValueResolver<GeneralObjectiveAggregate, GeneralObjectiveViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(GeneralObjectiveAggregate source, GeneralObjectiveViewModel destination, BasicViewModel destMember, ResolutionContext context) =>
                ExtractUser(source);

            private static BasicViewModel ExtractUser(GeneralObjectiveAggregate source)
            {
                var foundUser = source.Users.FirstOrDefault(c => c.Id == source.GeneralObjective.Objective.ModifiedBy);
                if (foundUser != null)
                {
                    return new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
                }

                foundUser = source.Users.FirstOrDefault(c => c.Id == source.GeneralObjective.Objective.CreatedBy);
                if (foundUser != null)
                {
                    return new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
                }

                return default;
            }
        }
    }
}