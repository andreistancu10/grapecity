using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Public.UploadFiles.Models;

namespace DigitNow.Domain.DocumentManagement.Public.UploadFiles.Mappings
{
    public class UploadFileMapping : Profile
    {
        public UploadFileMapping()
        {
            CreateMap<UploadFileRequest, UploadFileCommand>()
                .ForMember(c => c.TargetEntity, opt => opt.MapFrom(src => (TargetEntity)src.TargetEntity));
        }
    }
}