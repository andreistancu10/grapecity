using AutoMapper;
using DigitNow.Adapters.MS.Catalog.Poco;
using DigitNow.Domain.Authentication.Contracts;
using DigitNow.Domain.Catalog.Contracts.Departments.GetDepartments;
using DigitNow.Domain.Catalog.Contracts.DocumentTypes.GetDocumentTypes;
using DigitNow.Domain.Catalog.Contracts.InternalDocumentTypes;
using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models.Mappings
{
    public class ModelsMappings : Profile
    {
        public ModelsMappings()
        {
            CreateMap<IUserRoleModel, UserRoleModel>();
            CreateMap<IUserDepartmentModel, UserDepartmentModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.DepartmentId));

            CreateMap<IUserModel, UserModel>()
                .ForMember(x => x.Departments, opt => opt.MapFrom(o => o.UserDepartments))
                .ForMember(x => x.Roles, opt => opt.MapFrom(o => o.UserRoles));

            CreateMap<DepartmentDto, DocumentDepartmentModel>();
            CreateMap<IDepartmentResponse, DocumentDepartmentModel>();
            CreateMap<IDocumentTypeResponse, DocumentCategoryModel>();
            CreateMap<IInternalDocumentTypeResponse, InternalDocumentCategoryModel>();
            CreateMap<IInternalDocumentTypeResponse, DocumentCategoryModel>();
            CreateMap<SpecialRegisterMapping, SpecialRegisterMappingModel>();

            CreateMap<DocumentType, DocumentCategoryModel>();
            CreateMap<DocumentType, InternalDocumentCategoryModel>();
        }
    }
}