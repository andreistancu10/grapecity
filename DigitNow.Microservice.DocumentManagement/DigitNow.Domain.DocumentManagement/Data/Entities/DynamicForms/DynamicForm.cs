using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class DynamicForm : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Label { get; set; }
        public string Context { get; set; }

        public DynamicForm()
        {
        }

        public DynamicForm(long id)
        {
            Id = id;
        }
    }
}