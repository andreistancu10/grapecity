using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.DeliveryDetails;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Dtos.Mappings
{
    public class DtosMappings : Profile
    {
        public DtosMappings()
        {
            CreateMap<DeliveryDetail, DeliveryDetailDto>().ReverseMap();
            CreateMap<ConnectedDocument, ConnectedDocumentDto>().ReverseMap();
            CreateMap<WorkflowHistoryLog, WorkflowHistoryLogDto>().ReverseMap();
            CreateMap<ContactDetail, ContactDetailDto>().ReverseMap();
            CreateMap<DocumentUploadedFile, DocumentUploadedFileDto>().ReverseMap();
        }
    }
}
