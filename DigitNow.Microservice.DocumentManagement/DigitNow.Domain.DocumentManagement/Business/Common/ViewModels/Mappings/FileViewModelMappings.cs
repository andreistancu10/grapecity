using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class FileViewModelMappings : Profile
    {
        public FileViewModelMappings()
        {
            CreateMap<VirtualFileAggregate, FileViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UploadedFile.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UploadedFile.Name))
                .ForMember(dest => dest.UploadDate, opt => opt.MapFrom(src => src.UploadedFile.CreatedAt))
                .ForMember(dest => dest.UploadedBy, opt => opt.MapFrom<MapUploadedBy>());

            CreateMap<DocumentFileAggregate, DocumentFileViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DocumentFileModel.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DocumentFileModel.Name))
                .ForMember(dest => dest.UploadDate, opt => opt.MapFrom(src => src.DocumentFileModel.CreatedAt))
                .ForMember(dest => dest.UploadedBy, opt => opt.MapFrom<MapUploadedBy>())
                .ForMember(dest => dest.Category, opt => opt.MapFrom<MapCategory>());
        }

        private class MapUploadedBy :
            IValueResolver<VirtualFileAggregate, FileViewModel, BasicViewModel>,
            IValueResolver<DocumentFileAggregate, DocumentFileViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(VirtualFileAggregate source, FileViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                return FindUser(source.Users, source.UploadedFile.CreatedBy);
            }

            public BasicViewModel Resolve(DocumentFileAggregate source, DocumentFileViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                return FindUser(source.Users, source.DocumentFileModel.CreatedBy);
            }

            private static BasicViewModel FindUser(IReadOnlyList<UserModel> users, long userId)
            {
                var foundUser = users.FirstOrDefault(x => x.Id == userId);

                return foundUser != null ? new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}") : null;
            }
        }

        private class MapCategory :
            IValueResolver<DocumentFileAggregate, DocumentFileViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(DocumentFileAggregate source, DocumentFileViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                var documentCategoryId = source.DocumentFileMappings
                    .First(c => c.UploadedFileMappingId == source.DocumentFileModel.UploadedFileMappingId)
                    .DocumentCategoryId;

               var foundCategory = source.Categories.First(x => x.Id == documentCategoryId);

                return new BasicViewModel(foundCategory.Id, foundCategory.Name);
            }
        }
    }
}