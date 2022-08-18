using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Dtos.Mappings
{
    public class DtosMappings : Profile
    {
        public DtosMappings()
        {
            CreateMap<DeliveryDetail, DeliveryDetailDto>().ReverseMap();
            CreateMap<WorkflowHistoryLog, WorkflowHistoryLogDto>().ReverseMap();
            CreateMap<ContactDetail, ContactDetailDto>().ReverseMap();
            CreateMap<DocumentUploadedFile, DocumentUploadedFileDto>().ReverseMap();
            CreateMap<ObjectiveUploadedFile, ObjectiveUploadedFileDto>().ReverseMap();

            CreateMap<ConnectedDocument, ConnectedDocumentDto>()
                .ForMember(x => x.RegistrationNumber, opt => opt.MapFrom(src => src.Document.RegistrationNumber))
                .ForMember(x => x.DocumentType, opt => opt.MapFrom(src => src.Document.DocumentType));
        }
    }
}
