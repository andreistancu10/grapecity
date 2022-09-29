using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    public class RiskTrackingReportAggregate
    {
        public RiskTrackingReport RiskTrackingReport { get; set; }
        public IReadOnlyList<UserModel> Users { get; set; }
    }
}
