namespace DigitNow.Domain.DocumentManagement.Data.Entities.Actions
{
    public class ActionFunctionary: ExtendedEntity
    {
        public long ActionId { get; set; }
        public long FunctionaryId { get; set; }
        public Action Action { get; set; }
    }
}
