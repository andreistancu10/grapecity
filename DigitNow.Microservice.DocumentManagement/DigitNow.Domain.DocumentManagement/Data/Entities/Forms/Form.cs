using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class Form : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Label { get; set; }
        public string Context { get; set; }

        #region [ References ]

        //public List<FormField> FormFields { get; set; }
        //public List<FormValue> FormFieldValues { get; set; }
        //public List<FormFieldMapping> FormFieldMappings { get; set; }
        //public List<FormFillingLog> FormFillingLogs { get; set; }

        #endregion

        public Form()
        {
        }

        public Form(long id)
        {
            Id = id;
        }
    }
}