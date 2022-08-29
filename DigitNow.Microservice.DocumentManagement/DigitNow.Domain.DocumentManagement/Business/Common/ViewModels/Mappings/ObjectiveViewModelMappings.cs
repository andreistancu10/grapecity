using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class ObjectiveViewModelMappings : Profile
    {
        public ObjectiveViewModelMappings()
        {
            _ = CreateMap<VirtualObjectiveAgregate<SpecificObjective>, SpecificObjectiveViewModel>()
                .ForMember(c => c.Code, opt => opt.MapFrom(src => src.VirtualObjective.Objective.Code))
                .ForMember(c => c.Title, opt => opt.MapFrom(src => src.VirtualObjective.Objective.Title))
                .ForMember(c => c.CreatedAt, opt => opt.MapFrom(src => src.VirtualObjective.CreatedAt))
                .ForMember(c => c.ModifiedAt, opt => opt.MapFrom(src => src.VirtualObjective.Objective.SpecificObjective.ModifiedAt))
                .ForMember(c => c.State, opt => opt.MapFrom(src => src.VirtualObjective.Objective.State))
                .ForMember(c => c.Department, opt => opt.MapFrom<MapObjectiveDepartment>())
                .ForMember(c => c.Functionary, opt => opt.MapFrom<MapObjectiveFunctionary>())
                .ForMember(c => c.User, opt => opt.MapFrom<MapObjectiveUser>());
        }

        private class MapObjectiveDepartment :
            IValueResolver<VirtualObjectiveAgregate<SpecificObjective>, SpecificObjectiveViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(VirtualObjectiveAgregate<SpecificObjective> source, SpecificObjectiveViewModel destination, BasicViewModel destMember, ResolutionContext context) =>
                ExtractDepartment(source);

            private static BasicViewModel ExtractDepartment<T>(VirtualObjectiveAgregate<T> source)
                where T : VirtualObjective
            {
                var foundDepartment = source.Departments.FirstOrDefault(x => x.Id == source.VirtualObjective.Objective.SpecificObjective.DepartmentId);
                if (foundDepartment != null)
                {
                    return new BasicViewModel(foundDepartment.Id, foundDepartment.Name);
                }
                return default;
            }
        }

        private class MapObjectiveFunctionary :
            IValueResolver<VirtualObjectiveAgregate<SpecificObjective>, SpecificObjectiveViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(VirtualObjectiveAgregate<SpecificObjective> source, SpecificObjectiveViewModel destination, BasicViewModel destMember, ResolutionContext context) =>
                ExtractDepartment(source);

            private static BasicViewModel ExtractDepartment<T>(VirtualObjectiveAgregate<T> source)
                where T : VirtualObjective
            {
                var functionary = source.VirtualObjective.Objective.SpecificObjective.SpecificObjectiveFunctionarys?.Select(x => x.FunctionaryId).ToList();
                if (functionary != null)
                {
                    var foundUser = source.Users.FirstOrDefault(x => functionary.Contains(x.Id));
                    if (foundUser != null)
                    {
                        return new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
                    }
                }
                return default;
            }
        }

        private class MapObjectiveUser :
            IValueResolver<VirtualObjectiveAgregate<SpecificObjective>, SpecificObjectiveViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(VirtualObjectiveAgregate<SpecificObjective> source, SpecificObjectiveViewModel destination, BasicViewModel destMember, ResolutionContext context) =>
                ExtractDepartment(source);

            private static BasicViewModel ExtractDepartment<T>(VirtualObjectiveAgregate<T> source)
                where T : VirtualObjective
            {
                var foundUser = source.Users.FirstOrDefault(x => x.Id == source.VirtualObjective.CreatedBy);
                if (foundUser != null)
                {
                    return new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
                }
                return default;
            }
        }
    }
}
