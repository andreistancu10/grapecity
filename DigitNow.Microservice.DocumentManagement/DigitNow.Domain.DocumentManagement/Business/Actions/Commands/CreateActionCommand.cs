using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Actions.Commands
{
    public class CreateActionCommand : ICommand<ResultObject>
    {
        public long ActivityId { get; set; }
        public long DepartmentId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public List<long> ActionFunctionariesIds { get; set; }
        public List<long> UploadedFileIds { get; set; }
    }
}
