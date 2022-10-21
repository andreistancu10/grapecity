namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class ProcedureHistory : ExtendedEntity
    {
        public string Edition { get; set; }
        public string Revision { get; set; }
        public long ProcedureId { get; set; }
        public Procedure Procedure { get; set; }
    }
}