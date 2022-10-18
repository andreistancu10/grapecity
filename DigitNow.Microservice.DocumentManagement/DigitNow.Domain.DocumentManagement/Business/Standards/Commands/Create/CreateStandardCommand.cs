using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Standards.Commands.Create
{
    public class CreateStandardCommand : ICommand<ResultObject>
    {
        public string Title { get; set; }
        public string Activity { get; set; }
        public long DepartmentId { get; set; }
        public DateTime Deadline { get; set; }
        public string Observations { get; set; }
        public List<long> StandardFunctionariesIds { get; set; }
        public List<long> UploadedFileIds { get; set; }
    }
}
