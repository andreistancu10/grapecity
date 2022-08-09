using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries.GetById
{
    public class GetIncomingDocumentByIdMapping : Profile
    {
        public GetIncomingDocumentByIdMapping()
        {
            CreateMap<IncomingDocument, GetIncomingDocumentByIdResponse>()
                .ForMember(c => c.ConnectedDocuments, opt => opt.MapFrom(src => src.ConnectedDocuments))
                .ForMember(c => c.RegistrationNumber, opt => opt.MapFrom(src => src.Document.RegistrationNumber))
                .ForMember(c => c.RecipientId, opt => opt.MapFrom(src => src.Document.SourceDestinationDepartmentId))
                .ForPath(c => c.WorkflowHistory, opt => opt.MapFrom(src => src.Document.WorkflowHistories));
        }
    }
}