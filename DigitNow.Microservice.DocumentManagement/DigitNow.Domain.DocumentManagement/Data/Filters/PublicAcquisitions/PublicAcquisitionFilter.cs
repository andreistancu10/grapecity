namespace DigitNow.Domain.DocumentManagement.Data.Filters.PublicAcquisitions
{
    public class PublicAcquisitionFilter : DataFilter
    {
        public ProjectYearFilter ProjectYearFilter { get; set; }
        public TypeFilter TypeFilter { get; set; }
        public CpvCodeFilter CpvCodeFilter { get; set; }
        public FinancingSourceFilter FinancingSourceFilter { get; set; }
        public EstablishedProcedureFilter EstablishedProcedureFilter { get; set; }
        public EstimatedMonthForInitiatingProcedureFilter EstimatedMonthForInitiatingProcedureFilter { get; set; }
        public EstimatedMonthForProcedureAssignmentFilter EstimatedMonthForProcedureAssignmentFilter { get; set; }
        public ProcedureAssignmentMethodFilter ProcedureAssignmentMethodFilter { get; set; }
    }

    public class ProcedureAssignmentMethodFilter
    {
        public long ProcedureAssignmentMethod { get; set; }
    }

    public class EstimatedMonthForProcedureAssignmentFilter
    {
        public string EstimatedMonthForProcedureAssignment { get; set; }
    }

    public class EstimatedMonthForInitiatingProcedureFilter
    {
        public string EstimatedMonthForInitiatingProcedure { get; set; }
    }

    public class ProjectYearFilter
    {
        public int ProjectYear { get; set; }
    }

    public class TypeFilter
    {
        public string Type { get; set; }
    }

    public class CpvCodeFilter
    {
        public long CpvCode { get; set; }
    }

    public class FinancingSourceFilter
    {
        public long FinancingSource { get; set; }
    }

    public class EstablishedProcedureFilter
    {
        public long EstablishedProcedure { get; set; }
    }
}
