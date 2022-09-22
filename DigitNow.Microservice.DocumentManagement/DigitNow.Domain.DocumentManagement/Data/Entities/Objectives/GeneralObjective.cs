using DigitNow.Domain.DocumentManagement.Data.Entities.Procedures;
using DigitNow.Domain.DocumentManagement.Data.Entities.Risks;

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
