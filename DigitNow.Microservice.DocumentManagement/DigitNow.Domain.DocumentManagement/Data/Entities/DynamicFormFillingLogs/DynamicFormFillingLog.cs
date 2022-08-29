namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class DynamicFormFillingLog : ExtendedEntity
    {
        public long FormId { get; set; }

        #region [ References ]

        public DynamicForm DynamicForm { get; set; }

        #endregion
    }
}