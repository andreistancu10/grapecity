using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload;
using DigitNow.Domain.DocumentManagement.Data.Entities.UploadedFiles;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Mappings;

public class UploadFileMapping : Profile
{
    public UploadFileMapping()
    {
        CreateMap<UploadedFile, UploadFileResponse>()
            .ForMember(c => c.FileGuid, opt => opt.MapFrom(dest => dest.Guid))
            .ForMember(c => c.FileId, opt => opt.MapFrom(dest => dest.Id));
    }
}
