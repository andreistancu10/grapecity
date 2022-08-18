namespace DigitNow.Domain.DocumentManagement.Data.Entities.Objectives
{
    public class GeneralObjective: VirtualObjective
    {
        #region [ References ]
        public List<SpecificObjective> SpecificObjectives { get; set; }
        #endregion
    }
}
