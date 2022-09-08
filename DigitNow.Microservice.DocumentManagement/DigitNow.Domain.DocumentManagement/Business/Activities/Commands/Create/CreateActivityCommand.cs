using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Activities.Commands.Create
{
    public class CreateActivityCommand : ICommand<ResultObject>
    {
        public long GeneralObjectiveId { get; set; }
        public long SpecificObjectiveId { get; set; }
        public long DepartmentId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public List<long> UploadedFileIds { get; set; }
        public List<long> ActivityFunctionaryIds { get; set; }
    }
}
