using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Archive.Commands;
using DigitNow.Domain.DocumentManagement.Business.Archive.Historical.Commands.DeleteHistoricalArchiveDocument;
using DigitNow.Domain.DocumentManagement.Public.Archive.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Archive.Mappings
{
    public class DeleteDocumentMapping : Profile
    {
        public DeleteDocumentMapping()
        {
            CreateMap<DeleteDocumentRequest, DeleteDocumentCommand>();
            CreateMap<DeleteHistoricalArchiveDocumentRequest, DeleteHistoricalArchiveDocumentCommand>();
        }
    }
}
