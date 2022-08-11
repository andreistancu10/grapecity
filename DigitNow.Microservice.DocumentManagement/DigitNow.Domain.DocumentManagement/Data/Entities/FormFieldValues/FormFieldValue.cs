namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class FormFieldValue : ExtendedEntity
    {
        public string Value { get; set; }
        public long FormId { get; set; }
        public long FormFieldId { get; set; }

        #region [ References ]

        public FormField FormField { get; set; }
        public Form Form { get; set; }

        #endregion
    }
}