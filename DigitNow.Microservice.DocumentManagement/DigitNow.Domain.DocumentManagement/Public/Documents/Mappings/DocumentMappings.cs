using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Documents.Queries.SetDocumentsResolution;
using DigitNow.Domain.DocumentManagement.Public.Documents.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Documents.Mappings
{
    public class DocumentMappings : Profile
    {
        public DocumentMappings()
        {
            CreateMap<SetDocumentsResolutionRequest, SetDocumentsResolutionQuery>();
        }
    }
}
