using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class DynamicFormFillingLogModelMapping : Profile
    {
        public DynamicFormFillingLogModelMapping()
        {
            CreateMap<DynamicFormAggregate, DynamicFormFillingLogViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.DynamicFormFillingLog.Id))
                .ForMember(c => c.FormCreator, opt => opt.MapFrom<MapDynamicFormCreator>())
                .ForMember(c => c.Department, opt => opt.MapFrom<MapDynamicFormUserDepartment>())
                .ForMember(c => c.Category, opt => opt.MapFrom(src => src.DynamicFormFillingLog.DynamicForm.Name))
                .ForMember(c => c.CreatedAt, opt => opt.MapFrom(src => src.DynamicFormFillingLog.CreatedAt));
        }

        private class MapDynamicFormCreator :
             IValueResolver<DynamicFormAggregate, DynamicFormFillingLogViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(DynamicFormAggregate source, DynamicFormFillingLogViewModel destination, BasicViewModel destMember, ResolutionContext context)
            {
                var foundUser = source.Users.FirstOrDefault(x => x.Id == source.DynamicFormFillingLog.CreatedBy);

                if (foundUser != null)
                {
                    return new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
                }
                return default;
            }
        }

        private class MapDynamicFormUserDepartment :
                IValueResolver<DynamicFormAggregate, DynamicFormFillingLogViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(DynamicFormAggregate source, DynamicFormFillingLogViewModel destination, BasicViewModel destMember, ResolutionContext context)
            {
                var userDepartment = source.Departments.FirstOrDefault();

                if (userDepartment != null)
                {
                    return new BasicViewModel(userDepartment.Id, userDepartment.Name);
                }
                return default;
            }
        }
    }
}
