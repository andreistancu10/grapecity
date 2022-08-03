using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Mappings;

public class OutgoingDocumentMappings : Profile
{
    public OutgoingDocumentMappings()
    {
        CreateMap<CreateOutgoingDocumentCommand, OutgoingDocument>()
            .ForPath(c => c.Document.DestinationDepartmentId, opt => opt.MapFrom(src => src.DestinationDepartmentId));
    }
}