namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class ActivityFunctionary : ExtendedEntity
    {
        public long ActivityId { get; set; }
        public long FunctionaryId { get; set; }

        public Activity Activity { get; set; }
    }
}
