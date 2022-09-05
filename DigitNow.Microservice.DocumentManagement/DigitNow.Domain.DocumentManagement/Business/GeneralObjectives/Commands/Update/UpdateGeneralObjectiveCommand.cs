using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Commands.Update
{
    public class UpdateGeneralObjectiveCommand : ICommand<ResultObject>
    {
        public long ObjectiveId { get; set; }
        public ScimState State { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string ModificationMotive { get; set; }

        public List<long> UploadedFileIds { get; set; }
    }
}
