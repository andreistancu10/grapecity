using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Public.Actions.Models
{
    public class CreateActionRequest
    {
        public long ActivityId { get; set; }
        public long DepartmentId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public List<long> ActionFunctionariesIds { get; set; }
        public List<long> UploadedFileIds { get; set; }
    }
}
