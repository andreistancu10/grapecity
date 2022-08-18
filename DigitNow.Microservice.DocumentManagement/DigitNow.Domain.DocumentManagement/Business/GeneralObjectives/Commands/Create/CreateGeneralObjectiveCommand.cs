using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Commands.Create
{
    public class CreateGeneralObjectiveCommand : ICommand<ResultObject>
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public List<long> UploadedFileIds { get; set; }
    }
}
