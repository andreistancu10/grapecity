namespace DigitNow.Domain.DocumentManagement.Business.NotificationStatuses.Queries.GetById
{
    public class GetNotificationStatusByIdResponse
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public bool Active { get; set; }
    }
}