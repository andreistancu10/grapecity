namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class GetPublicAcquisitionProjectViewModel
    {
        public long Id { get; set; }
        public int ProjectYear { get; set; }
        public long Code { get; set; }
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
