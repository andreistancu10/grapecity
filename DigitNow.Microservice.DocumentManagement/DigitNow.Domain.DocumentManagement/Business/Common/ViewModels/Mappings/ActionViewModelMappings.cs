using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class ActionViewModelMappings : Profile
    {
        public ActionViewModelMappings()
        {
            CreateMap<ActionAggregate, ActionViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Action.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Action.Code))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Action.Title))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Action.State))
                .ForMember(dest => dest.ModificationMotive, opt => opt.MapFrom(src => src.Action.ModificationMotive))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Action.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.Action.ModifiedAt))
                .ForMember(dest => dest.SpecificObjective, opt => opt.MapFrom<MapActionSpecificObjective>())
                .ForMember(dest => dest.GeneralObjective, opt => opt.MapFrom<MapActionGeneralObjective>())
                .ForMember(dest => dest.Department, opt => opt.MapFrom<MapDepartment>())
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom<MapCreatedBy>())
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom<MapModifiedBy>())
                .ForMember(dest => dest.Activity, opt => opt.MapFrom<MapActivity>());
        }

        public class MapDepartment : IValueResolver<ActionAggregate, ActionViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(ActionAggregate source, ActionViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                var foundDepartment = source.Departments.FirstOrDefault(c => c.Id == source.Action.DepartmentId);

                return foundDepartment == null
                    ? null
                    : new BasicViewModel(foundDepartment.Id, foundDepartment.Name);
            }
        }

        public class MapCreatedBy : IValueResolver<ActionAggregate, ActionViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(ActionAggregate source, ActionViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                var foundUser = source.Users.FirstOrDefault(c => c.Id == source.Action.CreatedBy);

                return foundUser == null
                    ? null
                    : new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
            }
        }
        public class MapModifiedBy : IValueResolver<ActionAggregate, ActionViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(ActionAggregate source, ActionViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                var foundUser = source.Users.FirstOrDefault(c => c.Id == source.Action.ModifiedBy);

                return foundUser == null
                    ? null
                    : new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
            }
        }
    }

    public class MapActivity : IValueResolver<ActionAggregate, ActionViewModel, BasicViewModel>
    {
        public BasicViewModel Resolve(ActionAggregate source, ActionViewModel destination, BasicViewModel destMember,
            ResolutionContext context)
        {
            return new BasicViewModel(source.Action.Id, source.Action.AssociatedActivity.Title);
        }
    }

    public class MapActionSpecificObjective : IValueResolver<ActionAggregate, ActionViewModel, BasicViewModel>
    {
        public BasicViewModel Resolve(ActionAggregate source, ActionViewModel destination, BasicViewModel destMember,
            ResolutionContext context)
        {
            var foundSpecificObjective = source.SpecificObjectives.FirstOrDefault(c => c.ObjectiveId == source.Action.AssociatedActivity.SpecificObjectiveId);
            return foundSpecificObjective == null
                ? null
                : new BasicViewModel(foundSpecificObjective.Id, foundSpecificObjective.Title);
        }
    }

    public class MapActionGeneralObjective : IValueResolver<ActionAggregate, ActionViewModel, BasicViewModel>
    {
        public BasicViewModel Resolve(ActionAggregate source, ActionViewModel destination, BasicViewModel destMember,
            ResolutionContext context)
        {
            var foundGeneralObjective = source.GeneralObjectives.FirstOrDefault(c => c.ObjectiveId == source.Action.AssociatedActivity.GeneralObjectiveId);
            return foundGeneralObjective == null
                ? null
                : new BasicViewModel(foundGeneralObjective.Id, foundGeneralObjective.Title);
        }
    }
}