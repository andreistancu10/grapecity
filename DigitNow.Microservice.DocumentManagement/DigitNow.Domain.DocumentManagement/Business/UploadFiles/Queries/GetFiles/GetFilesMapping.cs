using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities.UploadedFiles;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.GetFiles
{
    public class GetFilesMapping : Profile
    {
        public GetFilesMapping()
        {
            CreateMap<UploadedFile, GetFilesResponse>()
                .ForMember(c=>c.Id, opt=>opt.MapFrom(dest=>dest.Id))
                .ForMember(c=>c.Name, opt=>opt.MapFrom(dest=>dest.Name))
                .ForMember(c=>c.UploadDate, opt=>opt.MapFrom(dest=>dest.CreatedAt))
                ;
        }
    }
}