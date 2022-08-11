using DigitNow.Domain.DocumentManagement.Data.Entities.Forms;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class FormField : ExtendedEntity
    {
        public long OrderNumber { get; set; }
        public FieldType FieldType { get; set; }
        public string Label { get; set; }
        public string Context { get; set; }
        public bool Required { get; set; }
        public string InitialValue { get; set; }
        public long FormId { get; set; }

        #region [ References ]

        public Form Form { get; set; }
        public List<FormFieldValue> FormFieldValues { get; set; }

        #endregion
    }
}