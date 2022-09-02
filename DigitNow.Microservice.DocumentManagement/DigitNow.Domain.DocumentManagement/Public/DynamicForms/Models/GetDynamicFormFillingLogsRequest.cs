namespace DigitNow.Domain.DocumentManagement.Public.DynamicForms.Models
{
    public class GetDynamicFormFillingLogsRequest
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public DynamicFormsFilterDto Filter { get; set; }
    }

    public class DynamicFormsFilterDto
    {
        public DynamicFormsRegistrationDateFilterDto DynamicFormsRegistrationDateFilter { get; set; }
        public DynamicFormCategoryFilterDto DynamicFormCategoryFilter { get; set; }
    }

    public class DynamicFormCategoryFilterDto
    {
        public int CategoryId { get; set; }
    }

    public class DynamicFormsRegistrationDateFilterDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
