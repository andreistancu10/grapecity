using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Archive.Historical.Commands.RemoveHistoricalArchiveDocument
{
    public class RemoveHistoricalArchiveDocumentCommand : ICommand<ResultObject>
    {
        public long Id { get; set; }
    }
}
