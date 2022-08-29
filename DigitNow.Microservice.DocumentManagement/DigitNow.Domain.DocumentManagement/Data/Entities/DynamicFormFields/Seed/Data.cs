using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.utils;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Seed
{
    public static partial class Data
    {
        public static IEnumerable<DynamicFormField> GetFormFields()
        {
            yield return Create(1, "Input", "", DynamicFieldType.Input);
            yield return Create(2, "Number", "", DynamicFieldType.Number);
            yield return Create(3, "Date", "", DynamicFieldType.Date);
            yield return Create(4, "CountryDropdown", "", DynamicFieldType.CountryDropdown);
            yield return Create(5, "DistrictDropdown", "", DynamicFieldType.DistrictDropdown);
            yield return Create(6, "CityDropdown", "", DynamicFieldType.CityDropdown);
        }

        private static DynamicFormField Create(long id, string name, string context, DynamicFieldType dynamicFieldType) => new(id)
        {
            Name = name,
            Context = context,
            DynamicFieldType = dynamicFieldType
        };
    }
}
