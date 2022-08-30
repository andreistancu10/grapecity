﻿using AutoMapper;
using DigitNow.Adapters.MS.Catalog.Poco;
using DigitNow.Domain.Authentication.Contracts;
using DigitNow.Domain.Catalog.Contracts.Departments.GetDepartments;
using DigitNow.Domain.Catalog.Contracts.DocumentTypes.GetDocumentTypes;
using DigitNow.Domain.Catalog.Contracts.InternalDocumentTypes;
using DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Newtonsoft.Json;

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

            CreateMap<DocumentFileMapping, DocumentFileMappingModel>();

            CreateMap<UploadFileCommand, FileModel>()
                .ForMember(c => c.ContentType, opt => opt.MapFrom(src => src.File.ContentType))
                .ForMember(c => c.OriginalFileName, opt => opt.MapFrom(src => src.File.FileName));

            CreateMap<UploadDocumentFileCommand, FileModel>()
                .ForMember(c => c.ContentType, opt => opt.MapFrom(src => src.File.ContentType))
                .ForMember(c => c.OriginalFileName, opt => opt.MapFrom(src => src.File.FileName));

            CreateMap<FileModel, DocumentFileModel>()
                .ForMember(dest => dest.DocumentCategoryId, opt => opt.MapFrom<MapDocumentCategoryId>())
                .ForMember(dest => dest.Name, opt => opt.MapFrom<MapName>());

            CreateMap<FileModel, StoredFileModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom<MapName>());

            CreateMap<StoredFileModel, DocumentFileModel>()
                .ForMember(dest => dest.DocumentCategoryId, opt => opt.MapFrom<MapDocumentCategoryId>())
                .ForMember(dest => dest.Name, opt => opt.MapFrom<MapName>());

            CreateMap<UploadedFile, DocumentFileModel>();
            CreateMap<DocumentFileModel, UploadedFile>();
            CreateMap<StoredFileModel, UploadedFile>();
        }

        public class MapDocumentCategoryId :
            IValueResolver<FileModel, DocumentFileModel, long>,
            IValueResolver<StoredFileModel, DocumentFileModel, long>
        {
            public long Resolve(FileModel source, DocumentFileModel destination, long destMember, ResolutionContext context)
            {
                return ExtractFromContext(source.Context);
            }

            public long Resolve(StoredFileModel source, DocumentFileModel destination, long destMember, ResolutionContext context)
            {
                return ExtractFromContext(source.Context);
            }

            private static long ExtractFromContext(string context)
            {
                var result = JsonConvert.DeserializeObject<IDictionary<string, object>>(context);
                const string documentCategoryIdKeyName = "documentCategoryId";

                if (result.ContainsKey(documentCategoryIdKeyName))
                {
                    return long.Parse(result[documentCategoryIdKeyName].ToString());
                }

                throw new InvalidOperationException("Invalid Context");
            }
        }

        public class MapName :
            IValueResolver<FileModel, DocumentFileModel, string>,
            IValueResolver<StoredFileModel, DocumentFileModel, string>,
            IValueResolver<FileModel, StoredFileModel, string>
        {
            public string Resolve(FileModel source, DocumentFileModel destination, string destMember, ResolutionContext context)
            {
                return ExtractFromContext(source.Context);
            }

            public string Resolve(StoredFileModel source, DocumentFileModel destination, string destMember, ResolutionContext context)
            {
                return ExtractFromContext(source.Context);
            }

            public string Resolve(FileModel source, StoredFileModel destination, string destMember, ResolutionContext context)
            {
                return ExtractFromContext(source.Context);
            }

            private static string ExtractFromContext(string context)
            {
                var result = JsonConvert.DeserializeObject<IDictionary<string, object>>(context);
                const string nameKey = "name";

                if (result.ContainsKey(nameKey))
                {
                    return result[nameKey].ToString();
                }

                throw new InvalidOperationException("Invalid Context");
            }
        }
    }
}