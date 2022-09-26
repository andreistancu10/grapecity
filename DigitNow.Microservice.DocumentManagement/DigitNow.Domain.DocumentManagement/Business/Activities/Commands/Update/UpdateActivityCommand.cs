using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Activities.Commands.Update
{
    public class UpdateActivityCommand : ICommand<ResultObject>
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public ScimState StateId { get; set; }
        public string ModificationMotive { get; set; }
        public List<long> UploadedFileIds { get; set; }
        public List<long> ActivityFunctionaryIds { get; set; }
    }
}
