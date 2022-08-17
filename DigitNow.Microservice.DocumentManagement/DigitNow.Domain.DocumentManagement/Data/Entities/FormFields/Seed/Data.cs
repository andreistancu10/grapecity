using DigitNow.Domain.DocumentManagement.utils;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Seed
{
    public static partial class Data
    {
        public static IEnumerable<FormField> GetFormFields()
        {
            yield return Create(1, "Input", "", FieldType.Input);
            yield return Create(2, "Number", "", FieldType.Number);
            yield return Create(3, "Date", "", FieldType.Date);
            yield return Create(4, "CountryDropdown", "", FieldType.CountryDropdown);
            yield return Create(5, "DistrictDropdown", "", FieldType.DistrictDropdown);
            yield return Create(6, "CityDropdown", "", FieldType.CityDropdown);
        }

        private static FormField Create(long id, string name, string context, FieldType fieldType) => new(id)
        {
            Name = name,
            Context = context,
            FieldType = fieldType
        };
    }
}
