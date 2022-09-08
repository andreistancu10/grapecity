using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Public.Activities.Models
{
    public class UpdateActivityRequest
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public ScimState State { get; set; }
        public string ModificationMotive { get; set; }
        public List<long> UploadedFileIds { get; set; }
        public List<long> ActivityFunctionaryIds { get; set; }
    }
}
