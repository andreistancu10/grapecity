using DigitNow.Domain.DocumentManagement.Data.Entities.Activities;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Objectives
{
    public class GeneralObjective: VirtualObjective
    {
        #region [ References ]
        public List<SpecificObjective> SpecificObjectives { get; set; }
        public List<Activity> Activities { get; set; }
        #endregion
    }
}
