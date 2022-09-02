using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.DynamicForms
{
    internal class DynamicFormsFilterBuilder : DataExpressionFilterBuilder<DynamicFormFillingLog, DynamicFormsFilter>
    {
        public DynamicFormsFilterBuilder(IServiceProvider serviceProvider, DynamicFormsFilter filter)
                : base(serviceProvider, filter) { }


        private void BuildFilterByDynamicFormCategory()
        {
            if (EntityFilter.DynamicFormCategoryFilter != null)
            {
                EntityPredicates.Add(dynamicForm => dynamicForm.DynamicFormId == EntityFilter.DynamicFormCategoryFilter.CategoryId);
            }
        }

        private void BuildFilterByRegistrationDate()
        {
            var registrationDateFilter = EntityFilter.DynamicFormsRegistrationDateFilter;

            if (registrationDateFilter != null)
            {
                EntityPredicates.Add(dynamicForm =>
                    dynamicForm.CreatedAt >= registrationDateFilter.From
                    &&
                    dynamicForm.CreatedAt <= registrationDateFilter.To
                );
            }
        }

        protected override void InternalBuild()
        {
            BuildFilterByRegistrationDate();
            BuildFilterByDynamicFormCategory();
        }
    }
}
