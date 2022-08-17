using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Commands.Create
{
    public class CreateSpecificObjectiveCommand : ICommand<ResultObject>
    {
        public long DepartmentId { get; set; }
        public long GeneralObjectiveId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string ModificationMotive { get; set; }
        public List<long> UploadedFileIds { get; set; }
        public List<long> SpecificObjectiveFunctionaryIds { get; set; }
    }
}
