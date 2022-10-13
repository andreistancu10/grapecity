namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class PublicAcquisitionProject : ExtendedEntity
    {
        public int ProjectYear { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public long CpvCode { get; set; }
        public float EstimatedValue { get; set; }
        public long FinancingSource { get; set; }
        public long EstablishedProcedure { get; set; }
        public string EstimatedMonthForInitiatingProcedure { get; set; }
        public string EstimatedMonthForProcedureAssignment { get; set; }
        public long ProcedureAssignmentMethod { get; set; }
        public long ProcedureAssignmentResponsible { get; set; }
    }
}
