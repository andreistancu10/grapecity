namespace DigitNow.Domain.DocumentManagement.Data.Entities.Seed
{
    public static partial class Data
    {
        public static IEnumerable<Form> GetForms()
        {
            yield return Create(1, "Formular 1", "description1", "label1", "");
            yield return Create(2, "Formular 2", "description2", "label2", "");
        }

        private static Form Create(long id, string name, string description, string label, string context) => new(id)
        {
            Name = name,
            Description = description,
            Label = label,
            Context = context
        };
    }
}
