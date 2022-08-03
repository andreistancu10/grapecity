using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Archive.Queries;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;
using DigitNow.Domain.DocumentManagement.Public.Dashboard.Models;
using System;

namespace DigitNow.Domain.DocumentManagement.Public.Dashboard.Mappings
{
    public class GetDocumentsMapping : Profile
    {
        public GetDocumentsMapping()
        {
            CreateMap<DocumentFilterDto, DocumentPreprocessFilter>()
                .ForMember(m => m.RegistryTypeFilter, opt => opt.MapFrom(src => src.RegistryTypeFilter))
                .ForMember(m => m.RegistrationNoFilter, opt => opt.MapFrom(src => src.RegistrationNoFilter))
                .ForMember(m => m.RegistrationDateFilter, opt => opt.MapFrom(src => src.RegistrationDateFilter))
                .ForMember(m => m.TypeFilter, opt => opt.MapFrom(src => src.DocumentTypeFilter))
                .ForMember(m => m.StatusFilter, opt => opt.MapFrom(src => src.DocumentStatusFilter))
                .ForMember(m => m.DepartmentFilter, opt => opt.MapFrom(src => src.DocumentDepartmentFilter))
                .ForMember(m => m.IdentifiersFilter, opt => opt.MapFrom(src => src.DocumentIdentifiersFilter));

            CreateMap<DocumentFilterDto, DocumentPostprocessFilter>()
                .ForMember(m => m.CategoryFilter, opt => opt.MapFrom(src => src.DocumentCategoryFilter))
                .ForMember(m => m.IdentificationNumberFilter, opt => opt.MapFrom(src => src.IdentificationNumberFilter));

            {
                CreateMap<DocumentRegistyTypeFilterDto, DocumentRegistryTypeFilter>()
                    .ForMember(m => m.RegistryTypes, opt => opt.MapFrom(src => src.RegistryTypes));

                CreateMap<DocumentRegistrationNoFilterDto, DocumentRegistrationNoFilter>()
                    .ForMember(m => m.From, opt => opt.MapFrom(src => src.From))
                    .ForMember(m => m.To, opt => opt.MapFrom(src => src.To));

                CreateMap<DocumentRegistrationDateFilterDto, DocumentRegistrationDateFilter>()
                    .ForMember(m => m.From, opt => opt.MapFrom(src => src.From))
                    .ForMember(m => m.To, opt => opt.MapFrom(src => src.To));

                CreateMap<DocumentTypeFilterDto, DocumentTypeFilter>()
                    .ForMember(m => m.DocumentType , opt => opt.MapFrom(src => src.DocumentType));

                CreateMap<DocumentCategoryFilterDto, DocumentCategoryFilter>()
                    .ForMember(m => m.CategoryIds, opt => opt.MapFrom(src => src.CategoryIds));

                CreateMap<DocumentStatusFilterDto, DocumentStatusFilter>()
                    .ForMember(m => m.Status, opt => opt.MapFrom(src => src.Status));

                CreateMap<DocumentDepartmentFilterDto, DocumentDepartmentFilter>()
                    .ForMember(m => m.DepartmentIds, opt => opt.MapFrom(src => src.DepartmentIds));

                CreateMap<DocumentIdentifiersFilterDto, DocumentIdentifiersFilter>()
                    .ForMember(m => m.Identifiers, opt => opt.MapFrom(src => src.Identifiers));
                
                CreateMap<DocumentIdentificationNumberFilterDto, DocumentIdentificationNumber>()
                    .ForMember(m => m.IdentificationNumber, opt => opt.MapFrom(src => src.IdentificationNumber));
            }
            CreateMap<GetDocumentsRequest, GetDocumentsQuery>()
                .ForMember(m => m.PreprocessFilter, opt => opt.MapFrom(src => src.Filter ?? new DocumentFilterDto()))
                .ForMember(m => m.PostprocessFilter, opt => opt.MapFrom(src => src.Filter ?? new DocumentFilterDto()));
            CreateMap<GetDocumentsRequest, GetDocumentsOperationalArchiveQuery>()
               .ForMember(m => m.PreprocessFilter, opt => opt.MapFrom(src => src.Filter ?? new DocumentFilterDto()))
               .ForMember(m => m.PostprocessFilter, opt => opt.MapFrom(src => src.Filter ?? new DocumentFilterDto()));
        }
    }
}
