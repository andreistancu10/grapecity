namespace DigitNow.Domain.DocumentManagement.Public.PublicAcquisitions.Models
{
    public class GetPublicAcquisitionsRequest
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public PublicAcquisitionFilterDto Filter { get; set; }
    }

    public class PublicAcquisitionFilterDto
    {
        public ProjectYearFilterDto ProjectYearFilter { get; set; }
        public TypeFilterDto TypeFilter { get; set; }
        public CpvCodeFilterDto CpvCodeFilter { get; set; }
        public FinancingSourceFilterDto FinancingSourceFilter { get; set; }
        public EstablishedProcedureFilterDto EstablishedProcedureFilter { get; set; }
        public EstimatedMonthForInitiatingProcedureFilterDto EstimatedMonthForInitiatingProcedureFilter { get; set; }
        public EstimatedMonthForProcedureAssignmentFilterDto EstimatedMonthForProcedureAssignmentFilter { get; set; }
        public ProcedureAssignmentMethodFilterDto ProcedureAssignmentMethodFilter { get; set; }
    }

    public class ProcedureAssignmentMethodFilterDto
    {
        public long ProcedureAssignmentMethod { get; set; }
    }

    public class EstimatedMonthForProcedureAssignmentFilterDto
    {
        public string EstimatedMonthForProcedureAssignment { get; set; }
    }

    public class EstimatedMonthForInitiatingProcedureFilterDto
    {
        public string EstimatedMonthForInitiatingProcedure { get; set; }
    }

    public class ProjectYearFilterDto
    {
        public int ProjectYear { get; set; }
    }

    public class TypeFilterDto
    {
        public string Type { get; set; }
    }

    public class CpvCodeFilterDto
    {
        public long CpvCode { get; set; }
    }

    public class FinancingSourceFilterDto
    {
        public long FinancingSource { get; set; }
    }

    public class EstablishedProcedureFilterDto
    {
        public long EstablishedProcedure { get; set; }
    }
}
