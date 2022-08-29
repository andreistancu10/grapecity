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
        public SpecialObjectiveCodeFilterDto CodeFilter { get; set; }
        public SpecialObjectiveTitleFilterDto TitleFilter { get; set; }
        public SpecialObjectiveDepartmentFilterDto DepartmentFilter { get; set; }
        public SpecialObjectiveFunctionaryFilterDto FunctionaryFilter { get; set; }
        public SpecialObjectiveStateFilterDto StateFilter { get; set; }
    }

    public class SpecialObjectiveCodeFilterDto
    {
        public string Code { get; set; }
    }

    public class SpecialObjectiveTitleFilterDto
    {
        public string Title { get; set; }
    }

    public class SpecialObjectiveDepartmentFilterDto
    {
        public long DepartmentId { get; set; }
    }

    public class SpecialObjectiveFunctionaryFilterDto
    {
        public long FunctionaryId { get; set; }
    }

    public class SpecialObjectiveStateFilterDto
    {
        public ObjectiveState StateId { get; set; }
    }
}
