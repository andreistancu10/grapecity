using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class FormFieldMapping : Entity
    {
        public string Key { get; set; }
        public string Label { get; set; }
        public string Context { get; set; }
        public int Order { get; set; }
        public bool Required { get; set; }
        public string InitialValue { get; set; }
        public long FormId { get; set; }
        public long FormFieldId { get; set; }
        //public long FormValueId { get; set; }

        #region [ References ]

        public Form Form { get; set; }
        public FormField FormField { get; set; }
        //public FormValue FormValue { get; set; }

        #endregion
    }
}