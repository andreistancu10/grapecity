namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class ProcedureFunctionary : ExtendedEntity
    {
        public long ProcedureId { get; set; }
        public long FunctionaryId { get; set; }

        public Procedure Procedure { get; set; }
    }
}
