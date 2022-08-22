﻿using System.Text.Json;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.FileUsageContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class FileViewModelMappings : Profile
    {
        public FileViewModelMappings()
        {
            CreateMap<VirtualFileAggregate, DocumentFileViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UploadedFile.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UploadedFile.Name))
                .ForMember(dest => dest.UploadDate, opt => opt.MapFrom(src => src.UploadedFile.CreatedAt))
                .ForMember(dest => dest.UploadedBy, opt => opt.MapFrom<MapUploadedBy>())
                .ForMember(dest => dest.Category, opt => opt.MapFrom<MapCategory>());
        }

        private class MapUploadedBy :
            IValueResolver<VirtualFileAggregate, DocumentFileViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(VirtualFileAggregate source, DocumentFileViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                var foundUser = source.Users.FirstOrDefault(x => x.Id == source.UploadedFile.CreatedBy);

                return foundUser != null ?
                    new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}") :
                    null;
            }
        }

        private class MapCategory :
            IValueResolver<VirtualFileAggregate, DocumentFileViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(VirtualFileAggregate source, DocumentFileViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                var usageContext = JsonSerializer.Deserialize<DocumentUsageContext>(source.UploadedFile.UsageContext, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });

                if (usageContext == null)
                {
                    return null;
                }

                var foundCategory = source.Categories.FirstOrDefault(x => x.Id == usageContext.DocumentCategoryId);

                return foundCategory != null ?
                    new BasicViewModel(foundCategory.Id, foundCategory.Name) :
                    null;
            }
        }
    }
}