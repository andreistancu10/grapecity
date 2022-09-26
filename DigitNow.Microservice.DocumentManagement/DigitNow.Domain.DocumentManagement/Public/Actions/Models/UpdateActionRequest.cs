using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Public.Actions.Models
{
    public class UpdateActionRequest
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long StateId{ get; set; }
        public string Details { get; set; }
        public List<long> ActionFunctionariesIds { get; set; }
        public List<long> UploadedFileIds { get; set; }
        public string ModificationMotive { get; set; }
    }
}
