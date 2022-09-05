namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class ActivityAction : ExtendedEntity
    {
        public long ActivityId { get; set; }

        #region References

        public Activity Activity { get; set; }

        #endregion
    }
}