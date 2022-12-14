using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class ActivityViewModelMappings : Profile
    {
        public ActivityViewModelMappings()
        {
            CreateMap<ActivityAggregate, ActivityViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Activity.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Activity.Code))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Activity.Title))
                .ForMember(dest => dest.State, opt => opt.MapFrom<MapState>())
                .ForMember(dest => dest.Detail, opt => opt.MapFrom(src => src.Activity.Details))
                .ForMember(dest => dest.ModificationMotive, opt => opt.MapFrom(src => src.Activity.ModificationMotive))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Activity.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.Activity.ModifiedAt ?? src.Activity.CreatedAt))
                .ForMember(dest => dest.GeneralObjective, opt => opt.MapFrom<MapGeneralObjective>())
                .ForMember(dest => dest.SpecificObjective, opt => opt.MapFrom<MapSpecificObjective>())
                .ForMember(dest => dest.Department, opt => opt.MapFrom<MapDepartment>())
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom<MapCreatedBy>())
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom<MapModifiedBy>());
        }

        private class MapState : IValueResolver<ActivityAggregate, ActivityViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(ActivityAggregate source, ActivityViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                var foundState = source.States.FirstOrDefault(x => x.Id == source.Activity.StateId);
                if (foundState != null)
                {
                    return new BasicViewModel(foundState.Id, foundState.Name);
                }
                return default;
            }
        }

        private class MapDepartment : IValueResolver<ActivityAggregate, ActivityViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(ActivityAggregate source, ActivityViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                var foundDepartment = source.Departments.FirstOrDefault(c => c.Id == source.Activity.DepartmentId);

                return foundDepartment == null
                    ? null
                    : new BasicViewModel(foundDepartment.Id, foundDepartment.Name);
            }
        }

        private class MapCreatedBy : IValueResolver<ActivityAggregate, ActivityViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(ActivityAggregate source, ActivityViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                var foundUser = source.Users.FirstOrDefault(c => c.Id == source.Activity.CreatedBy);

                return foundUser == null
                    ? null
                    : new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
            }
        }

        private class MapModifiedBy : IValueResolver<ActivityAggregate, ActivityViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(ActivityAggregate source, ActivityViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                var foundUser = source.Users.FirstOrDefault(c => c.Id == source.Activity.ModifiedBy);
                if (foundUser != null)
                {
                    return new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
                }

                foundUser = source.Users.FirstOrDefault(c => c.Id == source.Activity.CreatedBy);
                if (foundUser != null)
                {
                    return new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
                }

                return default;
            }
        }

        private class MapSpecificObjective : IValueResolver<ActivityAggregate, ActivityViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(ActivityAggregate source, ActivityViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                var foundSpecificObjective = source.SpecificObjectives.FirstOrDefault(c => c.ObjectiveId == source.Activity.SpecificObjectiveId);
                return foundSpecificObjective == null
                    ? null
                    : new BasicViewModel(foundSpecificObjective.ObjectiveId, foundSpecificObjective.Title);
            }
        }

        private class MapGeneralObjective : IValueResolver<ActivityAggregate, ActivityViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(ActivityAggregate source, ActivityViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                var foundGeneralObjective = source.GeneralObjectives.FirstOrDefault(c => c.ObjectiveId == source.Activity.GeneralObjectiveId);
                return foundGeneralObjective == null
                    ? null
                    : new BasicViewModel(foundGeneralObjective.ObjectiveId, foundGeneralObjective.Title);
            }
        }
    }
}