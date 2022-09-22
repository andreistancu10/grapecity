namespace DigitNow.Domain.DocumentManagement.Data.Entities.Procedures
{
    public class ProcedureFunctionary : ExtendedEntity
    {
        public long ProcedureId { get; set; }
        public long FunctionaryId { get; set; }

        public Procedure Procedure { get; set; }
    }
}
