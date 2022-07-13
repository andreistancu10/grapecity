using AutoMapper;
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
            CreateMap<DocumentFilterDto, DocumentFilter>();
            {
                CreateMap<DocumentRegistyTypeFilterDto, DocumentRegistryTypeFilter>();
                CreateMap<DocumentRegistrationNoFilterDto, DocumentRegistrationNoFilter>();
                CreateMap<DocumentRegistrationDateFilterDto, DocumentRegistrationDateFilter>();
                CreateMap<DocumentTypeFilterDto, DocumentTypeFilter>();
                CreateMap<DocumentCategoryFilterDto, DocumentCategoryFilter>();
                CreateMap<DocumentStatusFilterDto, DocumentStatusFilter>();
                CreateMap<DocumentIdentifiersFilterDto, DocumentIdentifiersFilter>();
            }
            CreateMap<GetDocumentsRequest, GetDocumentsQuery>();
        }
    }
}
