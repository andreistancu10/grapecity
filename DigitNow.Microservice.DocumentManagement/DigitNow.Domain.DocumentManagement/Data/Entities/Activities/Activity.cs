using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class Activity : ExtendedEntity
    {
        public long GeneralObjectiveId { get; set; }
        public long SpecificObjectiveId { get; set; }
        public long DepartmentId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public long StateId{ get; set; }
        public string Details { get; set; }
        public string ModificationMotive { get; set; }

        #region [ References ]
        public GeneralObjective AssociatedGeneralObjective { get; set; }
        public SpecificObjective AssociatedSpecificObjective { get; set; }
        public List<ActivityFunctionary> ActivityFunctionaries { get; set; }
        public List<Action> Actions { get; set; }
        public List<Risk> Risks { get; set; }
        public List<Procedure> Procedures { get; set; }
        public List<PerformanceIndicator> PerformanceIndicators { get; set; }

        #endregion
    }
}
