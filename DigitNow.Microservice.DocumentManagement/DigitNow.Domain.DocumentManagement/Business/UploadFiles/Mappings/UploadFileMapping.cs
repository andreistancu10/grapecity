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
                ;
            CreateMap<UploadedFile, UploadFileResponse>()
                .ForMember(c => c.FileId, opt => opt.MapFrom(dest => dest.Id));
        }
    }
}
