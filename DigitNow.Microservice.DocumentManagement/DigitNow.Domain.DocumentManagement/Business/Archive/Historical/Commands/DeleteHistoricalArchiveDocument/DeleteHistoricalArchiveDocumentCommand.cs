using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Archive.Historical.Commands.DeleteHistoricalArchiveDocument
{
    public class DeleteHistoricalArchiveDocumentCommand : ICommand<ResultObject>
    {
        public long Id { get; set; }
    }
}
