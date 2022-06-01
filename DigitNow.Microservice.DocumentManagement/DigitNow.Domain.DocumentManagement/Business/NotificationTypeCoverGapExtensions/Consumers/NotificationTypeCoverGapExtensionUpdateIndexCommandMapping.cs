using AutoMapper;
using DigitNow.Domain.DocumentManagement.Contracts.NotificationTypeCoverGapExtensions;
using DigitNow.Domain.DocumentManagement.Data.NotificationTypeCoverGapExtensions;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Consumers
{
    public class NotificationTypeCoverGapExtensionElasticUpdateIndexCommandMapping : Profile
    {
        public NotificationTypeCoverGapExtensionElasticUpdateIndexCommandMapping() => CreateMap<INotificationTypeCoverGapExtensionElasticUpdateIndexCommand, NotificationTypeCoverGapExtensionElastic>();
    }
}
