using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Maping;

public class InternalDocumentMappings: Profile
{
    public InternalDocumentMappings()
    {
        CreateMap<CreateInternalDocumentCommand, InternalDocument>()
            .ForPath(c => c.Document.DestinationDepartmentId, opt => opt.MapFrom(src => src.DestinationDepartmentId));
    }
}