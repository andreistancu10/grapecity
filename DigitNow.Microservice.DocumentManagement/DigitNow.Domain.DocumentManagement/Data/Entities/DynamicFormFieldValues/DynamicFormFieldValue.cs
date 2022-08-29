using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class DynamicFormFieldValue : Entity
    {
        public string Value { get; set; }
        public long FormFillingLogId { get; set; }
        public long FormFieldMappingId { get; set; }

        #region [ References ]

        public DynamicFormFieldMapping DynamicFormFieldMapping { get; set; }
        public DynamicFormFillingLog DynamicFormFillingLog { get; set; }

        #endregion
    }
}