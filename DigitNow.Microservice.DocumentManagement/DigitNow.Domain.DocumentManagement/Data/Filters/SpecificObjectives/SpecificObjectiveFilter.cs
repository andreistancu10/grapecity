using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.SpecificObjectives
{
    public class SpecificObjectiveFilter : DataFilter
    {
        public SpecialObjectiveCodeFilter CodeFilter { get; set; }
        public SpecialObjectiveTitleFilter TitleFilter { get; set; }
        public SpecialObjectiveDepartmentFilter DepartmentFilter { get; set; }
        public SpecialObjectiveFunctionaryFilter FunctionaryFilter { get; set; }
        public SpecialObjectiveStateFilter StateFilter { get; set; }

        public static SpecificObjectiveFilter Empty => new SpecificObjectiveFilter();
    }

    public class SpecialObjectiveCodeFilter
    {
        public string Code { get; set; }
    }

    public class SpecialObjectiveTitleFilter
    {
        public string Title { get; set; }
    }

    public class SpecialObjectiveDepartmentFilter
    {
        public long DepartmentId { get; set; }
    }

    public class SpecialObjectiveFunctionaryFilter
    {
        public long FunctionaryId { get; set; }
    }

    public class SpecialObjectiveStateFilter
    {
        public ObjectiveState StateId { get; set; }
    }
}
