using DigitNow.Domain.DocumentManagement.Data.Filters.DynamicForms;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.GetDynamicForms
{
    public class GetDynamicFormFillingLogsQuery : IQuery<GetDynamicFormFillingLogsResponse>
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public DynamicFormsFilter Filter { get; set; }
    }
}
