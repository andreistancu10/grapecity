using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Mappings;

public class IncomingDocumentMappings : Profile
{
    public IncomingDocumentMappings()
    {
        CreateMap<CreateIncomingDocumentCommand, IncomingDocument>();
    }
}