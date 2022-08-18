using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Commands.Update
{
    public class UpdateSpecificObjectiveCommand : ICommand<ResultObject>
    {
        public long ObjectiveId { get; set; }
        public string Title { get; set; }
        public ObjectiveState State { get; set; }
        public string Details { get; set; }
        public string ModificationMotive { get; set; }
        public List<long> UploadedFileIds { get; set; }
        public List<long> SpecificObjectiveFunctionaryIds { get; set; }
    }
}
