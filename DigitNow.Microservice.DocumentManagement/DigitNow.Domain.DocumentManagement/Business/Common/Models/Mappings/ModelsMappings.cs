using System;
using AutoMapper;
using DigitNow.Adapters.MS.Catalog.Poco;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.Catalog.Contracts.Departments.GetDepartments;
using DigitNow.Domain.Catalog.Contracts.DocumentTypes.GetDocumentTypes;
using DigitNow.Domain.Catalog.Contracts.InternalDocumentTypes;
using DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisterMappings;
using Domain.Authentication.Contracts;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models.Mappings
{
    public class ModelsMappings : Profile
    {
        public ModelsMappings()
        {
            CreateMap<IGetUserByIdResponse, UserModel>();
            CreateMap<User, UserModel>();

            CreateMap<DepartmentDto, DocumentDepartmentModel>();
            CreateMap<IDepartmentResponse, DocumentDepartmentModel>();
            CreateMap<IDocumentTypeResponse, DocumentCategoryModel>();
            CreateMap<IInternalDocumentTypeResponse, InternalDocumentCategoryModel>();
            CreateMap<IInternalDocumentTypeResponse, DocumentCategoryModel>();
            CreateMap<SpecialRegisterMapping, SpecialRegisterMappingModel>();
            CreateMap<SpecialRegisterMapping, DocumentsSpecialRegisterMappingModel>();

            CreateMap<DocumentType, DocumentCategoryModel>();
            CreateMap<DocumentType, InternalDocumentCategoryModel>();
        }
    }
}