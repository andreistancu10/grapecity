using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Public.GeneralObjectives.Models
{
    public class UpdateGeneralObjectiveRequest
    {
        public long ObjectiveId { get; set; }
        public ObjectiveState State { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string ModificationMotive { get; set; }
        public List<long> UploadedFileIds { get; set; }
    }
}
