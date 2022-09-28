using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class GeneralObjective: VirtualObjective
    {
        #region [ References ]

        public List<SpecificObjective> SpecificObjectives { get; set; }
        public List<Activity> Activities { get; set; }
        public List<Risk> Risks { get; set; }
        public List<Procedure> Procedures { get; set; }

        #endregion
    }
}
