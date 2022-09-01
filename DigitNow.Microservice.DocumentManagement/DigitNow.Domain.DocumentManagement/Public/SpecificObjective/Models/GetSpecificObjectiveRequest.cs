using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Public.SpecificObjective.Models
{
    public class GetSpecificObjectiveRequest
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public SpecificObjectiveFilterDto Filter { get; set; }
    }

    public class SpecificObjectiveFilterDto
    {
        public SpecificObjectiveGeneralObjectiveIdFilterDto GeneralObjectiveIdFilter { get; set; }
        public SpecificObjectiveCodeFilterDto CodeFilter { get; set; }
        public SpecificObjectiveTitleFilterDto TitleFilter { get; set; }
        public SpecificObjectiveDepartmentFilterDto DepartmentFilter { get; set; }
        public SpecificObjectiveFunctionaryFilterDto FunctionaryFilter { get; set; }
        public SpecificObjectiveStateFilterDto StateFilter { get; set; }
    }

    public class SpecificObjectiveGeneralObjectiveIdFilterDto
    {
        public long ObjectiveId { get; set; }
    }

    public class SpecificObjectiveCodeFilterDto
    {
        public string Code { get; set; }
    }

    public class SpecificObjectiveTitleFilterDto
    {
        public string Title { get; set; }
    }

    public class SpecificObjectiveDepartmentFilterDto
    {
        public long DepartmentId { get; set; }
    }

    public class SpecificObjectiveFunctionaryFilterDto
    {
        public long FunctionaryId { get; set; }
    }

    public class SpecificObjectiveStateFilterDto
    {
        public ObjectiveState StateId { get; set; }
    }
}
