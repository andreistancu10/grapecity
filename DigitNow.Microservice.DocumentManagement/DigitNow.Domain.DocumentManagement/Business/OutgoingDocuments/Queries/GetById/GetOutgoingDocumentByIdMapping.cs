using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetById
{
    public class GetOutgoingDocumentByIdMapping : Profile
    {
        public GetOutgoingDocumentByIdMapping()
        {
            CreateMap<OutgoingDocument, GetOutgoingDocumentByIdResponse>()
                .ForMember(c => c.ContactDetails, opt => opt.MapFrom(src => src.ContactDetail))
                .ForMember(c => c.DeliveryDetails, opt => opt.MapFrom(src => src.DeliveryDetails))
                .ForMember(c => c.WorkflowHistory, opt => opt.MapFrom(src => src.Document.WorkflowHistories))
                .ForMember(c => c.ConnectedDocuments, opt => opt.MapFrom(src => src.ConnectedDocuments))                
                .ForMember(c => c.RegistrationNumber, opt => opt.MapFrom(src => src.Document.RegistrationNumber))
                .ForMember(c => c.DestinationDepartmentId, opt => opt.MapFrom(src => src.Document.DestinationDepartmentId));
        }
    }
}