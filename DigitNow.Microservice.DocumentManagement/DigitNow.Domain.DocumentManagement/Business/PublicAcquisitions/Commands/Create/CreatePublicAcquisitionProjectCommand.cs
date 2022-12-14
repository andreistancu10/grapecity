using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.PublicAcquisitions.Commands.Create
{
    public class CreatePublicAcquisitionProjectCommand : ICommand<ResultObject>
    {
        public int ProjectYear { get; set; }
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
