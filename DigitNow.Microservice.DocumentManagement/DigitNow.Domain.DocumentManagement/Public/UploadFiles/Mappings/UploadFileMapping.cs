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
            CreateMap<UploadFileRequest, UploadDocumentFileCommand>()
                .ForMember(x => x.TargetEntity, opt => opt.MapFrom(src => (TargetEntity)src.TargetEntity));

            CreateMap<UploadFileRequest, UploadFileCommand>()
                .ForMember(x => x.TargetEntity, opt => opt.MapFrom(src => (TargetEntity)src.TargetEntity));
        }
    }
}