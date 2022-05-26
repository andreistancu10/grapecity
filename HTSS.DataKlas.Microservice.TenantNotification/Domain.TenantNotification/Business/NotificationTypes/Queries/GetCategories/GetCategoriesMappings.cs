using AutoMapper;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Helpers;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Queries.GetCategories
{
    public class GetCategoriesMappings : Profile
    {
        public GetCategoriesMappings()
        {
            CreateMap<NotificationTypeCategory, GetCategoriesResponse>();
        }
    }
}
