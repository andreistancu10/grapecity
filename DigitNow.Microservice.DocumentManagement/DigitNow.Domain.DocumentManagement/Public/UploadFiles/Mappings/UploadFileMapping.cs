using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload;
using DigitNow.Domain.DocumentManagement.Public.UploadFiles.Models;

namespace DigitNow.Domain.DocumentManagement.Public.UploadFiles.Mappings;

public class UploadFileMapping : Profile
{
    public UploadFileMapping()
    {
        CreateMap<UploadFileRequest, UploadFileCommand>();
    }
}