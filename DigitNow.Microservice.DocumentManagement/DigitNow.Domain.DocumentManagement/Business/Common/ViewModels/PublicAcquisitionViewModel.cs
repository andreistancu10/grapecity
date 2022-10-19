namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class PublicAcquisitionViewModel
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public BasicViewModel CpvCode { get; set; }
        public string Type { get; set; }
        public BasicViewModel EstablishedProcedure { get; set; }
    }
}
