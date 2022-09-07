using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Archive.Commands
{
    public class RemoveDocumentCommand : ICommand<ResultObject>
    {
        public long DocumentId { get; set; }
    }
}
