using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class FormFieldValue : Entity
    {
        public string Value { get; set; }
        public long FormFillingLogId { get; set; }
        public long FormFieldMappingId { get; set; }

        #region [ References ]

        public FormFieldMapping FormFieldMapping { get; set; }
        public FormFillingLog FormFillingLog { get; set; }

        #endregion
    }
}