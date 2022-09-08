using DigitNow.Domain.DocumentManagement.Contracts.DynamicForms;
using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class DynamicFormField : Entity
    {
        public DynamicFieldType DynamicFieldType { get; set; }
        public string Name { get; set; }
        public string Context { get; set; }

        public DynamicFormField() { }
        public DynamicFormField(long id)
        {
            Id = id;
        }
    }
}