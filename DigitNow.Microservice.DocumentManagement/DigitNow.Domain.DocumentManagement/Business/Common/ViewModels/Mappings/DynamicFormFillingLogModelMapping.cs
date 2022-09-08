using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using Newtonsoft.Json.Linq;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class DynamicFormFillingLogModelMapping : Profile
    {
        public DynamicFormFillingLogModelMapping()
        {
            CreateMap<DynamicFormAggregate, HistoricalArchiveDocumentsViewModel>()
                .ForMember(x => x.FormId, opt => opt.MapFrom(src => src.FormValues.DynamicForm.Id))
                .ForMember(x => x.FormFillingLogId, opt => opt.MapFrom(src => src.FormValues.Id))
                .ForMember(c => c.Category, opt => opt.MapFrom<MapCategory>())
                .ForMember(c => c.RegistrationAt, opt => opt.MapFrom(src => src.FormValues.CreatedAt))
                .ForMember(c => c.RegistrationBy, opt => opt.MapFrom<MapRegistrationBy>())
                .ForMember(c => c.Recipient, opt => opt.MapFrom<MapRecipient>());
        }

        private class MapRegistrationBy :
             IValueResolver<DynamicFormAggregate, HistoricalArchiveDocumentsViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(DynamicFormAggregate source, HistoricalArchiveDocumentsViewModel destination, BasicViewModel destMember, ResolutionContext context)
            {
                var foundUser = source.Users.FirstOrDefault(x => x.Id == source.FormValues.CreatedBy);
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
                //TODO: Change this after a user is assigned to multiple departments
                var userDepartment = source.Departments.FirstOrDefault();
                if (userDepartment != null)
                {
                    return new BasicViewModel(userDepartment.Id, userDepartment.Name);
                }
                return default;
            }
        }

        private class MapCategory
            : IValueResolver<DynamicFormAggregate, HistoricalArchiveDocumentsViewModel, BasicViewModel>
        {
            private const string TARGET_CONTEXT_KEY = "archivedDocumentCategoryId";

            public BasicViewModel Resolve(DynamicFormAggregate source, HistoricalArchiveDocumentsViewModel destination, BasicViewModel destMember, ResolutionContext context)
            {
                var formContext = source.FormValues.DynamicForm.Context;
                if (string.IsNullOrEmpty(formContext) || string.IsNullOrWhiteSpace(formContext))
                    return default;

                var formContextObject = JObject.Parse(formContext);
                if (formContextObject.ContainsKey(TARGET_CONTEXT_KEY))
                {
                    var foundValue = int.Parse(formContextObject[TARGET_CONTEXT_KEY].ToString());

                    var foundCategory = source.Categories.FirstOrDefault(x => x.Id == foundValue);
                    if (foundCategory != null)
                    {
                        return new BasicViewModel(foundCategory.Id, foundCategory.Name);
                    }
                }
                return default;
            }
        }
    }
}
