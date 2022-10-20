namespace DigitNow.Domain.DocumentManagement.Public.PerformanceIndicators.Models
{
    public class UpdatePerformanceIndicatorRequest
    {
        public long Id { get; set; }
        public long GeneralObjectiveId { get; set; }
        public long SpecificObjectiveId { get; set; }
        public long ActivityId { get; set; }
        public long DepartmentId { get; set; }
        public string Title { get; set; }
        public long StateId { get; set; }
        public string Target { get; set; }
        public string QuantificationFormula { get; set; }
        public string ResultIndicator { get; set; }
        public DateTime Deadline { get; set; }
        public string SolutionStage { get; set; }
        public string Observations { get; set; }
        public string ModificationMotive { get; set; }
        public List<long> PerformanceIndicatorFunctionariesIds { get; set; }
        public List<long> UploadedFileIds { get; set; }
    }
}
