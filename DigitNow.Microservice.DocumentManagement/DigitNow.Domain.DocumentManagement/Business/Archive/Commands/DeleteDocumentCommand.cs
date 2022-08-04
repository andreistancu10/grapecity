using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Archive.Commands
{
    public class DeleteDocumentCommand : ICommand<ResultObject>
    {
        public long DocumentId { get; set; }
    }
}
