using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class Activity : ExtendedEntity
    {
        public string ActivityCode { get; set; }
        public long SpecificObjectiveId { get; set; }
        public long DepartmentId { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }

        #region References

        public SpecificObjective SpecificObjective { get; set; }

        #endregion
    }
}