namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class StandardFunctionary: ExtendedEntity
    {
        public long StandardId { get; set; }
        public long FunctionaryId { get; set; }
        public Standard Standard { get; set; }
    }
}
