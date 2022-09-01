using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.SpecificObjectives
{
    public class SpecificObjectiveFilter : DataFilter
    {
        public SpecificObjectiveGeneralObjectiveIdFilter GeneralObjectiveIdFilter { get; set; }
        public SpecificObjectiveCodeFilter CodeFilter { get; set; }
        public SpecificObjectiveTitleFilter TitleFilter { get; set; }
        public SpecificObjectiveDepartmentFilter DepartmentFilter { get; set; }
        public SpecificObjectiveFunctionaryFilter FunctionaryFilter { get; set; }
        public SpecificObjectiveStateFilter StateFilter { get; set; }

        public static SpecificObjectiveFilter Empty => new SpecificObjectiveFilter();
    }

    public class SpecificObjectiveGeneralObjectiveIdFilter
    {
        public long ObjectiveId { get; set; }
    }

    public class SpecificObjectiveCodeFilter
    {
        public string Code { get; set; }
    }

    public class SpecificObjectiveTitleFilter
    {
        public string Title { get; set; }
    }

    public class SpecificObjectiveDepartmentFilter
    {
        public long DepartmentId { get; set; }
    }

    public class SpecificObjectiveFunctionaryFilter
    {
        public long FunctionaryId { get; set; }
    }

    public class SpecificObjectiveStateFilter
    {
        public ObjectiveState StateId { get; set; }
    }
}
