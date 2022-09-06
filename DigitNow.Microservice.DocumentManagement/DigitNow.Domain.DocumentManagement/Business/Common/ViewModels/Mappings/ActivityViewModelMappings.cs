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
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Activity.State.ToString()))
                .ForMember(dest => dest.Detail, opt => opt.MapFrom(src => src.Activity.Detail))
                .ForMember(dest => dest.ModificationMotive, opt => opt.MapFrom(src => src.Activity.ModificationMotive))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Activity.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.Activity.ModifiedAt))
                .ForMember(dest => dest.GeneralObjectiveId, opt => opt.MapFrom(src => src.Activity.GeneralObjectiveId))
                .ForMember(dest => dest.SpecificObjectiveId, opt => opt.MapFrom(src => src.Activity.SpecificObjectiveId))
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

                return foundUser == null
                    ? null
                    : new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
            }
        }
    }
}