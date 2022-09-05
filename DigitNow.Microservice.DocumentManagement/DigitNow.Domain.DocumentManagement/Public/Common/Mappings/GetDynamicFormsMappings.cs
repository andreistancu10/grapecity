using DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.GetDynamicForms;
using DigitNow.Domain.DocumentManagement.Data.Filters.DynamicForms;
using DigitNow.Domain.DocumentManagement.Public.DynamicForms.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Common.Mappings
{
    public class GetDynamicFormsMappings : AutoMapper.Profile
    {
        public GetDynamicFormsMappings()
        {
            CreateMap<GetDynamicFormFillingLogsRequest, GetDynamicFormFillingLogsQuery>()
                .ForMember(x => x.Filter, opt => opt.MapFrom(src => src.Filter ?? new DynamicFormsFilterDto()));

            CreateMap<DynamicFormsFilterDto, DynamicFormsFilter>();
            {
                CreateMap<DynamicFormsRegistrationDateFilterDto, DynamicFormsRegistrationDateFilter>()
                    .ForMember(m => m.From, opt => opt.MapFrom(src => src.From))
                    .ForMember(m => m.To, opt => opt.MapFrom(src => src.To));
                CreateMap<DynamicFormCategoryFilterDto, DynamicFormCategoryFilter>()
                    .ForMember(m => m.CategoryId, opt => opt.MapFrom(src => src.CategoryId));
            }
        }
    }
}
