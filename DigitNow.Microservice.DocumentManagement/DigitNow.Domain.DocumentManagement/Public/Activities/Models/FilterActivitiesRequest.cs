using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Public.Common.Models.FilterDtos;
using DigitNow.Domain.DocumentManagement.utils;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Public.Activities.Models
{
    public class FilterActivitiesRequest : AbstractFilterModel<Activity>
    {
        public int LanguageId { get; set; } = LanguagesUtils.RomanianLanguageId;
        public ActivityFilterDto Filter { get; set; }
    }

    public class ActivityFilterDto
    {
        public SpecificObjectivesFilterDto SpecificObjectivesFilter { get; set; }
        public ActivitiesFilterDto ActivitiesFilter { get; set; }
        public DepartmentsFilterDto DepartmentsFilter { get; set; }
        public FunctionariesFilterDto FunctionariesFilter { get; set; }
    }
}
