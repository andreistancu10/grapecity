namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class PerformanceIndicatorFunctionary : ExtendedEntity
    {
        public long PerformanceIndicatorId { get; set; }
        public long FunctionaryId { get; set; }

        public PerformanceIndicator PerformanceIndicator { get; set; }
    }
}
