using DigitNow.Domain.DocumentManagement.Data.Filters.DynamicForms;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.DynamicForms
{
    internal class DynamicFormsFilterComponentContext : DataExpressionFilterComponentContext
    {
        public DynamicFormsFilter DynamicFormFilter { get; set; }
    }
}
