using DigitNow.Domain.DocumentManagement.Data.Notifications;

namespace DigitNow.Domain.DocumentManagement.Business.Consumers
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