using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class Form : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Label { get; set; }
        public string Context { get; set; }

        public Form()
        {
        }

        public Form(long id)
        {
            Id = id;
        }
    }
}