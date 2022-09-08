using DigitNow.Domain.DocumentManagement.Data.Entities.Activities;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class SpecificObjective : VirtualObjective
    {
        public long DepartmentId { get; set; }
        public long GeneralObjectiveId { get; set; }

        #region [ References ]

        public GeneralObjective AssociatedGeneralObjective { get; set; }
        public List<SpecificObjectiveFunctionary> SpecificObjectiveFunctionarys { get; set; }
        public List<Activity> Activities { get; set; }

        #endregion

    }
}
