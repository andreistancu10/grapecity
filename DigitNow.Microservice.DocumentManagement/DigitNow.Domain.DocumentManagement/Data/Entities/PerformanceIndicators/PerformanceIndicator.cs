namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class PerformanceIndicator : ExtendedEntity
    {
        public long GeneralObjectiveId { get; set; }
        public long SpecificObjectiveId { get; set; }
        public long ActivityId { get; set; }
        public long DepartmentId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public long StateId { get; set; }
        public string Target { get; set; }
        public string QuantificationFormula { get; set; }
        public string ResultIndicator { get; set; }
        public DateTime Deadline { get; set; }
        public string SolutionStage { get; set; }
        public string Observations { get; set; }
        public string ModificationMotive { get; set; }

        #region [ References ]
        public GeneralObjective AssociatedGeneralObjective { get; set; }
        public SpecificObjective AssociatedSpecificObjective { get; set; }
        public Activity AssociatedActivity { get; set; }
        public List<PerformanceIndicatorFunctionary> PerformanceIndicatorFunctionaries { get; set; }
        #endregion

    }
}
