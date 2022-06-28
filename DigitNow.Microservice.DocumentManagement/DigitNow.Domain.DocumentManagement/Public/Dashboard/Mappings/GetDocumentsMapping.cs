using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;
using DigitNow.Domain.DocumentManagement.Public.Dashboard.Models;
using System;

namespace DigitNow.Domain.DocumentManagement.Public.Dashboard.Mappings
{
    public class GetDocumentsMapping : Profile
    {
        public GetDocumentsMapping()
        {
            CreateMap<GetDocumentsRequest, GetDocumentsQuery>();

                //.ForMember(dest => dest.Filter, opt => opt.MapFrom(FilterModelMappingFunction));
                //.ForMember(dest => dest.Filter.RegistrationNoFilterModel, opt => opt.MapFrom(x => x.Filter.RegistrationNoFilter))
                //.ForMember(dest => dest.Filter.RegistrationDateFilterModel, opt => opt.MapFrom(x => x.Filter.RegistrationDateFilter))
                //.ForMember(dest => dest.Filter.DocumentType, opt => opt.MapFrom(x => x.Filter.DocumentType))
                //.ForMember(dest => dest.Filter.DocumentCategoryFilterModel, opt => opt.MapFrom(x => x.Filter.DocumentCategoryFilter))
                //.ForMember(dest => dest.Filter.DocumentState, opt => opt.MapFrom(x => x.Filter.DocumentState));
        }

        //private DocumentsFilterModel FilterModelMappingFunction(GetDocumentsRequest request, GetDocumentsQuery query)
        //{
        //    return new DocumentsFilterModel
        //    {
        //        RegistryType = request.Filter.RegistryType,
        //        RegistrationNoFilter = new RegistrationNoFilterModel
        //        {
        //            From = request.Filter.RegistrationNoFilter.From,
        //            To = request.Filter.RegistrationNoFilter.To
        //        },
        //        RegistrationDateFilter = new RegistrationDateFilterModel
        //        {
        //            From = request.Filter.RegistrationDateFilter.From,
        //            To = request.Filter.RegistrationDateFilter.To
        //        },
        //        DocumentType = request.Filter.DocumentType,
        //        DocumentCategoryFilter = new DocumentCategoryFilterModel
        //        {
        //            CategoryId = request.Filter.DocumentCategoryFilter.CategoryId
        //        },
        //        DocumentState = request.Filter.DocumentState
        //    };
        //}

    }
}
