using ShiftIn.Domain.TenantNotification.Data.Notifications;

namespace ShiftIn.Domain.TenantNotification.Business.Consumers
{
    public interface INotificationElasticUpdateIndexCommand : INotificationElastic
    {
        public enum SynchronizationStrategyCode
        {
            All = 1,
            ById = 2
        }

        public SynchronizationStrategyCode SynchronizationStrategy { get; }
    }
}