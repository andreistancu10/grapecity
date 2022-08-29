using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class DynamicFormFieldValue : Entity
    {
        public string Value { get; set; }
        public long DynamicFormFillingLogId { get; set; }
        public long DynamicFormFieldMappingId { get; set; }

        #region [ References ]

        public DynamicFormFieldMapping DynamicFormFieldMapping { get; set; }
        public DynamicFormFillingLog DynamicFormFillingLog { get; set; }

        #endregion
    }
}