namespace DigitNow.Domain.DocumentManagement.Data.Entities.Seed
{
    public static class DynamicFormData
    {
        public static IEnumerable<DynamicForm> GetForms()
        {
            yield return Create(1, "Formular 1", "description1", "label1", "");
            yield return Create(2, "Formular 2", "description2", "label2", "");
        }

        private static DynamicForm Create(long id, string name, string description, string label, string context) => new(id)
        {
            Name = name,
            Description = description,
            Label = label,
            Context = context
        };
    }
}
