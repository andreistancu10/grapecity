using System.Text.Json;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload;
using DigitNow.Domain.DocumentManagement.Data;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Mappings
{
    public class UploadFileMapping : Profile
    {
        public UploadFileMapping()
        {
            CreateMap<UploadFileCommand, UploadedFile>()
                .ForMember(c => c.ContentType, opt => opt.MapFrom(src => src.File.ContentType))
                .ForMember(c => c.UsageContext, opt => opt.MapFrom<MapUsageContext>())
                ;
            CreateMap<UploadedFile, UploadFileResponse>()
                .ForMember(c => c.FileId, opt => opt.MapFrom(dest => dest.Id));
        }

        private class MapUsageContext : IValueResolver<UploadFileCommand, UploadedFile, string>
        {
            public string Resolve(UploadFileCommand source, UploadedFile destination, string destMember, ResolutionContext context)
            {
                return JsonSerializer.Serialize(source.UsageContext);
            }
        }
    }
}
