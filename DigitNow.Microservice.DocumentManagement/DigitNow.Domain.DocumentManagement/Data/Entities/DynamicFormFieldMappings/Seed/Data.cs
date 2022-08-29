namespace DigitNow.Domain.DocumentManagement.Data.Entities.Seed
{
    public static partial class Data
    {
        public static IEnumerable<DynamicFormFieldMapping> GetFormFieldMappings()
        {
            yield return Create(1, "lastName", "Nume", "", 1, true, "", 1, 1);
            yield return Create(2, "firstName", "Prenume", "", 2, true, "", 1, 1);
            yield return Create(3, "resolutionPeriod", "Termen Solutionare", "", 3, false, "7", 1, 2);
            yield return Create(4, "createdDate", "Data Creare", "", 4, true, "", 1, 3);

            yield return Create(5, "observations", "Observatii", "", 1, true, "", 2, 1);
            yield return Create(6, "countryId", "Tara", "", 2, false, "161", 2, 4);
            yield return Create(7, "cityId", "Oras", "", 3, false, "", 2, 6);
            yield return Create(8, "districtId", "Judet", "", 4, true, "", 2, 5);
        }

        private static DynamicFormFieldMapping Create(long id, string key, string label, string context, int order, bool required, string initialValue, long formId, long formFieldId) =>
            new(id)
            {
                Key = key,
                Label = label,
                Context = context,
                Order = order,
                Required = required,
                InitialValue = initialValue,
                FormId = formId,
                FormFieldId = formFieldId
            };
    }
}