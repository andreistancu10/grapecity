using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class RiskViewModel
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public BasicViewModel SpecificObjective { get; set; }
        public BasicViewModel Department { get; set; }
        public string Description { get; set; }
        public ScimState State { get; set; }
        public DateTime DateOfLastRevision { get; set; }
    }
}
