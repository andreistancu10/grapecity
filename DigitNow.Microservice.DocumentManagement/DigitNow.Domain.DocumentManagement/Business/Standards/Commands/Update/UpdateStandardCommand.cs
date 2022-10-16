using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Standards.Commands.Update
{
    public class UpdateStandardCommand : ICommand<ResultObject>
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Activity { get; set; }
        public long StateId { get; set; }
        public long DepartmentId { get; set; }
        public DateTime Deadline { get; set; }
        public string Observations { get; set; }
        public List<long> StandardFunctionariesIds { get; set; }
        public List<long> UploadedFileIds { get; set; }
        public string ModificationMotive { get; set; }
    }
}
