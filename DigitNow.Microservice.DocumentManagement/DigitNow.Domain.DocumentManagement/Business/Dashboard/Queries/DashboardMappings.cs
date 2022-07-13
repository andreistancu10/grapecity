using System.Linq;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;

public class GetDocumentsMappings : Profile
{
    public GetDocumentsMappings()
    {
        CreateMap<IncomingDocument, GetDocumentResponse>()
            .ForMember(c => c.RegistrationDate, opt => opt.MapFrom(src => src.Document.RegistrationDate))
            .ForMember(c => c.RegistrationNumber, opt => opt.MapFrom(src => src.Document.RegistrationNumber))
            .ForMember(c => c.IssuerName, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(c => c.Status, opt => opt.MapFrom<GetDocumentResponseIncomingStatusValueResolver>())
            .ForMember(c => c.DocumentCategory, opt => opt.MapFrom(src => src.DocumentTypeId))
            .ForMember(c => c.DocumentType, opt => opt.MapFrom<GetDocumentResponseIncomingDocumentTypeValueResolver>())
            .ForMember(c => c.DocumentCategory, opt => opt.MapFrom(src => src.DocumentTypeId))
            .ForMember(c => c.ResolutionPeriod, opt => opt.MapFrom(src => src.ResolutionPeriod));

        CreateMap<OutgoingDocument, GetDocumentResponse>()
            .ForMember(c => c.RegistrationDate, opt => opt.MapFrom(src => src.Document.RegistrationDate))
            .ForMember(c => c.RegistrationNumber, opt => opt.MapFrom(src => src.Document.RegistrationNumber))
            .ForMember(c => c.RecipientName, opt => opt.MapFrom(src => src.RecipientName))
            .ForMember(c => c.IssuerName, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(c => c.Status, opt => opt.MapFrom<GetDocumentResponseOutgoingStatusValueResolver>())
            .ForMember(c => c.DocumentCategory, opt => opt.MapFrom(src => src.DocumentTypeId))
            .ForMember(c => c.DocumentType, opt => opt.MapFrom<GetDocumentResponseOutgoingDocumentTypeValueResolver>())
            .ForMember(c => c.DocumentCategory, opt => opt.MapFrom(src => src.DocumentTypeId));

        CreateMap<InternalDocument, GetDocumentResponse>()
            .ForMember(c => c.RegistrationDate, opt => opt.MapFrom(src => src.Document.RegistrationDate))
            .ForMember(c => c.RegistrationNumber, opt => opt.MapFrom(src => src.Document.RegistrationNumber))
            .ForMember(c => c.IssuerName, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(c => c.DocumentCategory, opt => opt.MapFrom(src => src.InternalDocumentTypeId))
            .ForMember(c => c.DocumentType, opt => opt.MapFrom(src => src.Document.DocumentType.ToString()));
    }

    private class GetDocumentResponseOutgoingStatusValueResolver : IValueResolver<OutgoingDocument, GetDocumentResponse, int>
    {
        public int Resolve(OutgoingDocument source, GetDocumentResponse destination, int destMember, ResolutionContext context)
        {
            return
                (int)source.WorkflowHistory
                    .OrderByDescending(c => c.CreatedAt)
                    .First()
                    .Status;
        }
    }

    private class GetDocumentResponseOutgoingDocumentTypeValueResolver : IValueResolver<OutgoingDocument, GetDocumentResponse, string>
    {
        public string Resolve(OutgoingDocument source, GetDocumentResponse destination, string destMember, ResolutionContext context)
        {
            var workflowStatus = source.WorkflowHistory
                .OrderByDescending(c => c.CreatedAt)
                .First()
                .Status;

            if ((DocumentStatus)destination.Status == DocumentStatus.Finalized
                || (DocumentStatus)workflowStatus == DocumentStatus.Finalized)
            {
                return "dms.dashboard.incoming-to-outgoing";
            }

            return DocumentType.Outgoing.ToString();
        }
    }

    private class GetDocumentResponseIncomingStatusValueResolver : IValueResolver<IncomingDocument, GetDocumentResponse, int>
    {
        public int Resolve(IncomingDocument source, GetDocumentResponse destination, int destMember, ResolutionContext context)
        {
            return
                (int)source.WorkflowHistory
                    .OrderByDescending(c => c.CreatedAt)
                    .First()
                    .Status;
        }
    }

    private class GetDocumentResponseIncomingDocumentTypeValueResolver : IValueResolver<IncomingDocument, GetDocumentResponse, string>
    {
        public string Resolve(IncomingDocument source, GetDocumentResponse destination, string destMember, ResolutionContext context)
        {
            var workflowStatus = source.WorkflowHistory
                .OrderByDescending(c => c.CreatedAt)
                .First()
                .Status;

            if ((DocumentStatus)destination.Status == DocumentStatus.Finalized
                || (DocumentStatus)workflowStatus == DocumentStatus.Finalized)
            {
                return "dms.dashboard.incoming-to-outgoing";
            }

            return DocumentType.Incoming.ToString();
        }
    }
}