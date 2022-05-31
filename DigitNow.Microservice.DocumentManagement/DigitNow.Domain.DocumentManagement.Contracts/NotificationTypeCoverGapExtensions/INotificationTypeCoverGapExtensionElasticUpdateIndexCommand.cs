namespace DigitNow.Domain.DocumentManagement.Contracts.NotificationTypeCoverGapExtensions
{
    public interface INotificationTypeCoverGapExtensionElasticUpdateIndexCommand : INotificationTypeCoverGapExtensionElastic
    {
        public enum SynchronizationStrategyCode
        {
            All = 1,
            ById = 2,
        }

        SynchronizationStrategyCode SynchronizationStrategy { get; }
    }
}
