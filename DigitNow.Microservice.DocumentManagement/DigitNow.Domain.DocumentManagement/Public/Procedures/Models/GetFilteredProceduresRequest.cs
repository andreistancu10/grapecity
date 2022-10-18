using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.Procedures;

namespace DigitNow.Domain.DocumentManagement.Public.Procedures.Models
{
    public class GetFilteredProceduresRequest
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public ProceduresFilterDto Filter { get; set; }
    }

    public class ProceduresFilterDto
    {
        public GeneralObjectivesFilterDto GeneralObjectivesFilter { get; set; }
        public SpecificObjectivesFilterDto SpecificObjectivesFilter { get; set; }
        public ActivitiesFilterDto ActivitiesFilter { get; set; }
        public ProcedureNameFilterDto ProcedureNameFilter { get; set; }
        public ProcedureStateFilterDto ProcedureStateFilter { get; set; }
        public ProcedureCategoriesFilterDto ProcedureCategoriesFilter { get; set; }
        public DepartmentsFilterDto DepartmentsFilter { get; set; }
        public FunctionaryFilterDto FunctionaryFilter { get; set; }
        public StartDateFilterDto StartDateFilter { get; set; }
    }

    public class GeneralObjectivesFilterDto
    {
        public List<long> GeneralObjectiveIds { get; set; }
    }

    public class SpecificObjectivesFilterDto
    {
        public List<long> SpecificObjectiveIds { get; set; }
    }

    public class ActivitiesFilterDto
    {
        public List<long> ActivityIds { get; set; }
    }

    public class ProcedureNameFilterDto
    {
        public string ProcedureName { get; set; }
    }

    public class ProcedureStateFilterDto
    {
        public ScimState ProcedureState { get; set; }
    }

    public class ProcedureCategoriesFilterDto
    {
        public List<ProcedureCategory> ProcedureCategories { get; set; }
    }
    public class DepartmentsFilterDto
    {
        public List<long> DepartmentIds { get; set; }
    }
    public class FunctionaryFilterDto
    {
        public long FunctionaryId { get; set; }
    }
    public class StartDateFilterDto
    {
        public DateTime StartDate { get; set; }
    }
}
