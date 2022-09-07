using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Archive.Commands;
using DigitNow.Domain.DocumentManagement.Business.Archive.Historical.Commands.RemoveHistoricalArchiveDocument;
using DigitNow.Domain.DocumentManagement.Public.Archive.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Archive.Mappings
{
    public class RemoveDocumentMapping : Profile
    {
        public RemoveDocumentMapping()
        {
            CreateMap<RemoveDocumentRequest, RemoveDocumentCommand>();
            CreateMap<RemoveHistoricalArchiveDocumentRequest, RemoveHistoricalArchiveDocumentCommand>();
        }
    }
}
