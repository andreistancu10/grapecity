namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class SpecificObjectiveFunctionary : ExtendedEntity
    {
        public long SpecificObjectiveId { get; set; }
        public long FunctionaryId { get; set; }

        public SpecificObjective SpecificObjective { get; set; }
    }
}
