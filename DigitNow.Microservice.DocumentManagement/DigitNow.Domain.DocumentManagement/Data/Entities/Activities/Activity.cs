using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Data.Entities.Activities;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class Activity : ExtendedEntity
    {
        public long GeneralObjectiveId { get; set; }
        public long SpecificObjectiveId { get; set; }
        public long DepartmentId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public ScimState State { get; set; }
        public string Detail { get; set; }
        public string ModificationMotive { get; set; }

        #region [ References ]
        public GeneralObjective AssociatedGeneralObjective { get; set; }
        public SpecificObjective AssociatedSpecificObjective { get; set; }
        public List<ActivityFunctionary> ActivityFunctionarys { get; set; }
        public List<Action> Actions { get; set; }
        #endregion
    }
}
