using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.DeliveryDetails;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Dtos.Mappings
{
    public class DtosMappings : Profile
    {
        public DtosMappings()
        {
            CreateMap<DeliveryDetail, DeliveryDetailDto>();
            CreateMap<ConnectedDocument, ConnectedDocumentDto>();
            CreateMap<WorkflowHistoryLog, WorkflowHistoryLogDto>();
            CreateMap<ContactDetail, ContactDetailDto>();
            CreateMap<DocumentUploadedFile, DocumentUploadedFileDto>();
        }
    }
}
