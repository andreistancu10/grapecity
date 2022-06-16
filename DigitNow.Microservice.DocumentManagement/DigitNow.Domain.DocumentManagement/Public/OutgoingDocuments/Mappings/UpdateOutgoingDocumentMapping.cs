using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Update;
using DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Models;
using DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments.Models;

namespace DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments.Mappings;

public class UpdateOutgoingDocumentMapping : Profile
{
    public UpdateOutgoingDocumentMapping()
    {
        CreateMap<UpdateOutgoingDocumentRequest, UpdateOutgoingDocumentCommand>();
    }
}