using DigitNow.Domain.DocumentManagement.utils;
using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class FormField : Entity
    {
        public DynamicFieldType DynamicFieldType { get; set; }
        public string Name { get; set; }
        public string Context { get; set; }
        
        public FormField()
        {
        }

        public FormField(long id)
        {
            Id = id;
        }
    }
}