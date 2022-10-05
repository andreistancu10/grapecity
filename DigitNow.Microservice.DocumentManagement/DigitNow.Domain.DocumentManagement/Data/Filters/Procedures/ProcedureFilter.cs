using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.Procedures;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Procedures
{
    public class ProcedureFilter : DataFilter
    {
        public GeneralObjectivesFilter GeneralObjectivesFilter { get; set; }
        public SpecificObjectivesFilter SpecificObjectivesFilter { get; set; }
        public ActivitiesFilter ActivitiesFilter { get; set; }
        public ProcedureNameFilter ProcedureNameFilter { get; set; }
        public ProcedureStateFilter ProcedureStateFilter { get; set; }
        public ProcedureCategoriesFilter ProcedureCategoriesFilter { get; set; }
        public DepartmentsFilter DepartmentsFilter { get; set; }
        public StartDateFilter StartDateFilter { get; set; }

        public static ProcedureFilter Empty => new();
    }

    public class GeneralObjectivesFilter
    {
        public List<long> GeneralObjectiveIds { get; set; }
    }

    public class SpecificObjectivesFilter
    {
        public List<long> SpecificObjectiveIds { get; set; }
    }

    public class ActivitiesFilter
    {
        public List<long> ActivityIds { get; set; }
    }

    public class ProcedureNameFilter
    {
        public string ProcedureName { get; set; }
    }

    public class ProcedureStateFilter
    {
        public ScimState ProcedureState { get; set; }
    }

    public class ProcedureCategoriesFilter
    {
        public List<ProcedureCategory> ProcedureCategories { get; set; }
    }
    public class DepartmentsFilter
    {
        public List<long> DepartmentIds { get; set; }
    }

    public class StartDateFilter
    {
        public DateTime StartDate { get; set; }
    }
}
