namespace DigitNow.Domain.DocumentManagement.Public.PerformanceIndicators.Models
{
    public class CreatePerformanceIndicatorRequest
    {
        public long GeneralObjectiveId { get; set; }
        public long SpecificObjectiveId { get; set; }
        public long ActivityId { get; set; }
        public long DepartmentId { get; set; }
        public string Title { get; set; }
        public string Target { get; set; }
        public string QuantificationFormula { get; set; }
        public string ResultIndicator { get; set; }
        public DateTime Deadline { get; set; }
        public string SolutionStage { get; set; }
        public string Observations { get; set; }
        public List<long> PerformanceIndicatorFunctionariesIds { get; set; }
        public List<long> UploadedFileIds { get; set; }
    }
}
