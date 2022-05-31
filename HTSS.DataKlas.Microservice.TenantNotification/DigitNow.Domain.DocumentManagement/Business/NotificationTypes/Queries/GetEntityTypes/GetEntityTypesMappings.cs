using AutoMapper;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Helpers;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Queries.GetEntityTypes
{
    public class GetEntityTypesMappings : Profile
    {
        public GetEntityTypesMappings()
        {
            CreateMap<NotificationEntityType, GetEntityTypesResponse>();
        }
    }
}
