using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Activities
{
    public class ActivityFilter : AbstractFilterModel<Activity>, IQuery<PagedList<ActivityViewModel>>
    {
        public long? Id { get; set; }
        public SpecificObjectivesFilterDto SpecificObjectivesFilter { get; set; }
        public ActivitiesFilterDto ActivitiesFilter { get; set; }
        public DepartmentsFilterDto DepartmentsFilter { get; set; }
        public FunctionariesFilterDto FunctionariesFilter { get; set; }
    }
}
}