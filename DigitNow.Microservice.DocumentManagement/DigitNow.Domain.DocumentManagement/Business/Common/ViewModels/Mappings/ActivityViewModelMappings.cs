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
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Activity.State))
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

        public class MapDepartment : IValueResolver<ActivityAggregate, ActivityViewModel, BasicViewModel>
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

        public class MapCreatedBy : IValueResolver<ActivityAggregate, ActivityViewModel, BasicViewModel>
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
        public class MapModifiedBy : IValueResolver<ActivityAggregate, ActivityViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(ActivityAggregate source, ActivityViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                var foundUser = source.Users.FirstOrDefault(c => c.Id == source.Activity.ModifiedBy);
                if (foundUser == null)
                {
                    var user = source.Users.FirstOrDefault(c => c.Id == source.Activity.CreatedBy);
                    return new BasicViewModel(user.Id, $"{user.FirstName} {user.LastName}");
                }

                return  new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
            }
        }
    }

    public class MapSpecificObjective : IValueResolver<ActivityAggregate, ActivityViewModel, BasicViewModel>
    {
        public BasicViewModel Resolve(ActivityAggregate source, ActivityViewModel destination, BasicViewModel destMember,
            ResolutionContext context)
        {
            var foundSpecificObjective = source.SpecificObjectives.FirstOrDefault(c => c.Id == source.Activity.SpecificObjectiveId);
            return foundSpecificObjective == null
                ? null
                : new BasicViewModel(foundSpecificObjective.Id, foundSpecificObjective.Title);
        }
    }

    public class MapGeneralObjective : IValueResolver<ActivityAggregate, ActivityViewModel, BasicViewModel>
    {
        public BasicViewModel Resolve(ActivityAggregate source, ActivityViewModel destination, BasicViewModel destMember,
            ResolutionContext context)
        {
            var foundGeneralObjective = source.GeneralObjectives.FirstOrDefault(c => c.Id == source.Activity.GeneralObjectiveId);
            return foundGeneralObjective == null
                ? null
                : new BasicViewModel(foundGeneralObjective.Id, foundGeneralObjective.Title);
        }
    }
}