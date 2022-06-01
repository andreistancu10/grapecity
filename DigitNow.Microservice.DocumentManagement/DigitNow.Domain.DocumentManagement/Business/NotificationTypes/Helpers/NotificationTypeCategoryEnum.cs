using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Helpers
{
    public enum NotificationTypeCategoryEnum
    {
        Informative = 1,
        Reactive = 2
    }

    public class NotificationTypeCategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public static class NotificationTypeCategoryHelper
    {
        public static IEnumerable<NotificationTypeCategory> GetNotificationTypeCategories()
        {
            yield return new NotificationTypeCategory
            {
                Id = (long) NotificationTypeCategoryEnum.Informative,
                Name = nameof(NotificationTypeCategoryEnum.Informative)
            };

            yield return new NotificationTypeCategory
            {
                Id = (long) NotificationTypeCategoryEnum.Reactive,
                Name = nameof(NotificationTypeCategoryEnum.Reactive)
            };
        }
    }
}