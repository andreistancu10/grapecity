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
                .ForMember(dest => dest.State, opt => opt.MapFrom<MapState>())
                .ForMember(dest => dest.ModificationMotive, opt => opt.MapFrom(src => src.Action.ModificationMotive))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Action.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.Action.ModifiedAt ?? src.Action.CreatedAt))
                .ForMember(dest => dest.SpecificObjective, opt => opt.MapFrom<MapActionSpecificObjective>())
                .ForMember(dest => dest.GeneralObjective, opt => opt.MapFrom<MapActionGeneralObjective>())
                .ForMember(dest => dest.Department, opt => opt.MapFrom<MapDepartment>())
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom<MapCreatedBy>())
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom<MapModifiedBy>())
                .ForMember(dest => dest.Activity, opt => opt.MapFrom<MapActivity>());
        }

        private class MapState : IValueResolver<ActionAggregate, ActionViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(ActionAggregate source, ActionViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                var foundState = source.States.FirstOrDefault(x => x.Id == source.Action.StateId);
                if (foundState != null)
                {
                    return new BasicViewModel(foundState.Id, foundState.Name);
                }
                return default;
            }
        }

        private class MapDepartment : IValueResolver<ActionAggregate, ActionViewModel, BasicViewModel>
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

        private class MapCreatedBy : IValueResolver<ActionAggregate, ActionViewModel, BasicViewModel>
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

        private class MapModifiedBy : IValueResolver<ActionAggregate, ActionViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(ActionAggregate source, ActionViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                var foundUser = source.Users.FirstOrDefault(c => c.Id == source.Action.ModifiedBy);
                if (foundUser != null)
                {
                    return new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
                }

                foundUser = source.Users.FirstOrDefault(c => c.Id == source.Action.CreatedBy);
                if (foundUser != null)
                {
                    return new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
                }

                return default;
            }
        }

        private class MapActivity : IValueResolver<ActionAggregate, ActionViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(ActionAggregate source, ActionViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                return new BasicViewModel(source.Action.Id, source.Action.AssociatedActivity.Title);
            }
        }

        private class MapActionSpecificObjective : IValueResolver<ActionAggregate, ActionViewModel, BasicViewModel>
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

        private class MapActionGeneralObjective : IValueResolver<ActionAggregate, ActionViewModel, BasicViewModel>
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
}