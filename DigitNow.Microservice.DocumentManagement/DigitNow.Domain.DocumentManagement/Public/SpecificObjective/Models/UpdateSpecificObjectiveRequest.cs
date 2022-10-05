using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Public.SpecificObjectives.Models
{
    public class UpdateSpecificObjectiveRequest
    {
        public long ObjectiveId { get; set; }
        public string Title { get; set; }
        public long StateId{ get; set; }
        public string Details { get; set; }
        public string ModificationMotive { get; set; }
        public List<long> UploadedFileIds { get; set; }
        public List<long> SpecificObjectiveFunctionaryIds { get; set; }
    }
}
