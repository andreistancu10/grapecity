using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Activities;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.Activities.Queries.FilterActivities
{
    public class FilterActivitiesQuery: IQuery<ResultPagedList<ActivityViewModel>>
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public ActivityFilter Filter { get; set; }
    }
}
