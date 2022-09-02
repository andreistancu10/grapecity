namespace DigitNow.Domain.DocumentManagement.Data.Filters.DynamicForms
{
    public class DynamicFormsFilter : DataFilter
    {
        public DynamicFormsRegistrationDateFilter DynamicFormsRegistrationDateFilter { get; set; }
        public DynamicFormCategoryFilter DynamicFormCategoryFilter { get; set; }
    }

    public class DynamicFormCategoryFilter
    {
        public int CategoryId { get; set; }
    }

    public class DynamicFormsRegistrationDateFilter
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
