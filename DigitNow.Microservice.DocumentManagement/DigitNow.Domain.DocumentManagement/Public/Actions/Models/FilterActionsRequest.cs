using DigitNow.Domain.DocumentManagement.Public.Common.Models.FilterDtos;
using DigitNow.Domain.DocumentManagement.utils;
using HTSS.Platform.Infrastructure.Data.Abstractions;
using Action = DigitNow.Domain.DocumentManagement.Data.Entities.Action;

namespace DigitNow.Domain.DocumentManagement.Public.Actions.Models
{
    public class FilterActionsRequest : AbstractFilterModel<Action>
    {
        public int LanguageId { get; set; } = LanguagesUtils.RomanianLanguageId;
        public ActionFilterDto Filter { get; set; }
    }

    public class ActionFilterDto
    {
        public SpecificObjectiveFilterDto SpecificObjectiveFilter { get; set; }
        public ActionsFilterDto ActionsFilter { get; set; }
        public DepartmentsFilterDto DepartmentsFilter { get; set; }
        public FunctionariesFilterDto FunctionariesFilter { get; set; }
        public ActivitiesFilterDto ActivitiesFilter { get; set; }
    }

    public class ActionsFilterDto
    {
        public List<long> ActionIds { get; set; }
    }
}
