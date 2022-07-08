using System;
using AutoMapper;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.Catalog.Contracts.DocumentTypes.GetDocumentTypes;
using DigitNow.Domain.Catalog.Contracts.InternalDocumentTypes;
using Domain.Authentication.Contracts;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models.Mappings;

public class ModelsMappings : Profile
{
    public ModelsMappings()
    {
        CreateMap<IGetUserByIdResponse, UserModel>();
        CreateMap<User, UserModel>();

        CreateMap<IDocumentTypeResponse, DocumentCategoryModel>();
        CreateMap<IInternalDocumentTypeResponse, InternalDocumentCategoryModel>();
    }
}