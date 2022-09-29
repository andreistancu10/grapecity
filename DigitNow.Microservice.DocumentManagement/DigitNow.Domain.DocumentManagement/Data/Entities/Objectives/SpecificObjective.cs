using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class SpecificObjective : VirtualObjective
    {
        public long DepartmentId { get; set; }
        public long GeneralObjectiveId { get; set; }

        #region [ References ]

        public GeneralObjective AssociatedGeneralObjective { get; set; }
        public List<SpecificObjectiveFunctionary> SpecificObjectiveFunctionaries { get; set; }
        public List<Activity> Activities { get; set; }
        public List<Risk> Risks { get; set; }
        public List<Procedure> Procedures { get; set; }

        #endregion

    }
}
