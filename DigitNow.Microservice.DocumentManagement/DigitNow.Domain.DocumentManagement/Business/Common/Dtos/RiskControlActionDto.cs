namespace DigitNow.Domain.DocumentManagement.Business.Common.Dtos
{
    public class RiskControlActionDto
    {
        public long? Id { get; set; }
        public string ControlMeasurement { get; set; }
        public string Deadline { get; set; }
    }
}
