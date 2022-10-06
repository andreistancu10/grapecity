using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class DynamicFormFieldMapping : Entity
    {
        public string Key { get; set; }
        public string Label { get; set; }
        public string Context { get; set; }
        public int Order { get; set; }
        public bool Required { get; set; }
        public string InitialValue { get; set; }
        public long DynamicFormId { get; set; }
        public long DynamicFormFieldId { get; set; }

        #region [ References ]

        public DynamicForm DynamicForm { get; set; }
        public DynamicFormField DynamicFormField { get; set; }
        public ICollection<DynamicFormFieldValue> DynamicFormFieldValues { get; set; }

        #endregion

        public DynamicFormFieldMapping() { }
        public DynamicFormFieldMapping(long id)
        {
            Id = id;
        }
    }
}