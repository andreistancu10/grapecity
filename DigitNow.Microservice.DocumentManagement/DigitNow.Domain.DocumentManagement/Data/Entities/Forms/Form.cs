namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class Form : ExtendedEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Label { get; set; }
        public string Context { get; set; }


        #region [ References ]

        public List<FormField> FormFields { get; set; }
        public List<FormFieldValue> FormFieldValues { get; set; }


        #endregion
    }
}