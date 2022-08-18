namespace DigitNow.Domain.DocumentManagement.Public.SpecialObjective.Models
{
    public class CreateSpecificObjectiveRequest
    {
        public long DepartmentId { get; set; }
        public long GeneralObjectiveId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public List<long> UploadedFileIds { get; set; }
        public List<long> SpecificObjectiveFunctionaryIds { get; set; }
    }
}
