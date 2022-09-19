using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Activities;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters.Activities;
using DigitNow.Domain.DocumentManagement.utils;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.Activities.Queries.FilterActivities
{
    public class FilterActivitiesQuery : AbstractFilterModel<Activity>, IQuery<ResultPagedList<ActivityViewModel>>
    {
        public int LanguageId { get; set; } = LanguagesUtils.RomanianLanguageId;
        public ActivityFilter Filter { get; set; }
    }
}
