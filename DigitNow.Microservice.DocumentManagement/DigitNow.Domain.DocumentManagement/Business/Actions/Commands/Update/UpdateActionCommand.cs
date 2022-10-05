using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Actions.Commands.Update
{
    public class UpdateActionCommand : ICommand<ResultObject>
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long StateId { get; set; }
        public string Details { get; set; }
        public List<long> ActionFunctionariesIds { get; set; }
        public List<long> UploadedFileIds { get; set; }
        public string ModificationMotive { get; set; }
    }
}
