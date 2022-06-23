using System.Linq;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
using DigitNow.Domain.DocumentManagement.Data.InternalDocuments;
using DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;

public class GetDocumentsMappings : Profile
{
    public GetDocumentsMappings()
    {
        CreateMap<IncomingDocument, GetDocumentResponse>()
            .ForMember(dest => dest.Status,
                opts => opts.MapFrom(src =>
                    ((Status) src.WorkflowHistory
                        .OrderByDescending(wh => wh.CreationDate)
                        .FirstOrDefault()
                        .Status).ToString()))
            .ForMember(dest => dest.DocumentType, opts => opts.MapFrom<IncomingDocumentTypeResolver>());

        CreateMap<OutgoingDocument, GetDocumentResponse>()
            .ForMember(dest => dest.Status,
                opts => opts.MapFrom(src =>
                    ((Status)src.WorkflowHistory
                        .OrderByDescending(wh => wh.CreationDate)
                        .FirstOrDefault()
                        .Status).ToString()))
            .ForMember(dest => dest.DocumentType, opts => opts.MapFrom<OutgoingDocumentTypeResolver>());

        CreateMap<InternalDocument, GetDocumentResponse>()
            .BeforeMap((s, d) => d.DocumentType = (int)DocumentType.Internal);
    }

    public class IncomingDocumentTypeResolver : IValueResolver<IncomingDocument, GetDocumentResponse, int>
    {
        public int Resolve(IncomingDocument source, GetDocumentResponse destination, int destMember, ResolutionContext context)
        {
            if (destination.Status == (int) Status.finalized &&
                source.DocumentTypeId is (int) DocumentType.Incoming or (int) DocumentType.Outgoing)
            {
                return (int) DocumentType.IncomingToOutgoing;
            }

            return source.DocumentTypeId;
        }
    }

    public class OutgoingDocumentTypeResolver : IValueResolver<OutgoingDocument, GetDocumentResponse, int>
    {
        public int Resolve(OutgoingDocument source, GetDocumentResponse destination, int destMember, ResolutionContext context)
        {
            if (destination.Status == (int) Status.finalized &&
                source.DocumentTypeId is (int) DocumentType.Incoming or (int) DocumentType.Outgoing)
            {
                return (int) DocumentType.IncomingToOutgoing;
            }

            return source.DocumentTypeId;
        }
    }
}
