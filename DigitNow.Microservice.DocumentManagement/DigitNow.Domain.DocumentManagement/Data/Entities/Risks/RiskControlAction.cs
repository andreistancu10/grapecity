namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class RiskControlAction : ExtendedEntity
    {
        public long RiskId { get; set; }
        public string ControlMeasurement { get; set; }
        public string Deadline { get; set; }

        public Risk Risk { get; set; }
    }
}
