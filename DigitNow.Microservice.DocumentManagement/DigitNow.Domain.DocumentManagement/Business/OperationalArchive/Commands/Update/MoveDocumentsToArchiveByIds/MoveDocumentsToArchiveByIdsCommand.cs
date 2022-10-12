using HTSS.Platform.Core.CQRS;


namespace DigitNow.Domain.DocumentManagement.Business.OperationalArchive.Commands.Update.MoveDocumentsToArchiveByIds
{
    public class MoveDocumentsToArchiveByIdsCommand : ICommand<ResultObject>
    {
        public List<long> DocumentIds { get; set; }
    }
}
