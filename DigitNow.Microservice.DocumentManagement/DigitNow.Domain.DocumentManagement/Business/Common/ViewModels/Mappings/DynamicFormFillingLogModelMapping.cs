using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class DynamicFormFillingLogModelMapping : Profile
    {
        public DynamicFormFillingLogModelMapping()
        {
            CreateMap<DynamicFormAggregate, HistoricalArchiveDocumentsViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.DynamicFormValues.Id))
                .ForMember(c => c.Category, opt => opt.MapFrom(src => src.DynamicFormValues.DynamicForm.Name))
                .ForMember(c => c.RegistrationAt, opt => opt.MapFrom(src => src.DynamicFormValues.CreatedAt))
                .ForMember(c => c.RegistrationBy, opt => opt.MapFrom<MapRegistrationBy>())
                .ForMember(c => c.Recipient, opt => opt.MapFrom<MapRecipient>());                
        }

        private class MapRegistrationBy :
             IValueResolver<DynamicFormAggregate, HistoricalArchiveDocumentsViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(DynamicFormAggregate source, HistoricalArchiveDocumentsViewModel destination, BasicViewModel destMember, ResolutionContext context)
            {
                var foundUser = source.Users.FirstOrDefault(x => x.Id == source.DynamicFormValues.CreatedBy);
                if (foundUser != null)
                {
                    return new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
                }
                return default;
            }
        }

        private class MapRecipient :
                IValueResolver<DynamicFormAggregate, HistoricalArchiveDocumentsViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(DynamicFormAggregate source, HistoricalArchiveDocumentsViewModel destination, BasicViewModel destMember, ResolutionContext context)
            {
                //TODO: Review this
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
