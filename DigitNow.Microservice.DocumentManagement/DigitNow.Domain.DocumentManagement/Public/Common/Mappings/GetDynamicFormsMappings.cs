using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.GetDynamicForms;
using DigitNow.Domain.DocumentManagement.Data.Filters.DynamicForms;
using DigitNow.Domain.DocumentManagement.Public.DynamicForms.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Common.Mappings
{
    public class GetDynamicFormsMappings : Profile
    {
        public GetDynamicFormsMappings()
        {
            CreateMap<GetHistoricalArchiveDocumentsRequest, GetHistoricalArchiveDocumentsQuery>()
                .ForMember(x => x.Filter, opt => opt.MapFrom(src => src.Filter ?? new HistoricalArchiveDocumentsFilterDto()));

            CreateMap<HistoricalArchiveDocumentsFilterDto, DynamicFormsFilter>();
            {
                CreateMap<HistoricalArchiveDocumentsRegistrationDateFilterDto, DynamicFormsRegistrationDateFilter>()
                    .ForMember(m => m.From, opt => opt.MapFrom(src => src.From))
                    .ForMember(m => m.To, opt => opt.MapFrom(src => src.To));
                CreateMap<HistoricalArchiveDocumentsCategoryFilterDto, DynamicFormCategoryFilter>()
                    .ForMember(m => m.CategoryId, opt => opt.MapFrom(src => src.CategoryId));
            }
        }
    }
}
