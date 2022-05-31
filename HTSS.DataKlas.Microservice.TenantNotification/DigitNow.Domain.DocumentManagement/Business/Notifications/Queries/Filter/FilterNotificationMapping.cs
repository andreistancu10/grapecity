using System.Globalization;
using AutoMapper;
using ShiftIn.Domain.TenantNotification.Data.Notifications;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Queries.Filter
{
    public class FilterNotificationMapping : Profile
    {
        public FilterNotificationMapping()
        {
            CreateMap<NotificationElastic, FilterNotificationResponse>()
                .ForMember(dest => dest.CreatedOn,
                    opt => opt.MapFrom(src =>
                        src.CreatedOn.ToString("MMMM d, yyyy - hh\\:mm", CultureInfo.CreateSpecificCulture("en-US"))))
                .ForMember(dest => dest.ModifiedOn,
                    opt => opt.MapFrom(src =>
                        src.ModifiedOn.HasValue
                            ? src.ModifiedOn.Value.ToString("MMMM d, yyyy - hh\\:mm",
                                CultureInfo.CreateSpecificCulture("en-US"))
                            : ""))
                .ForMember(dest => dest.SeenOn,
                    opt => opt.MapFrom(src =>
                        src.SeenOn.HasValue
                            ? src.SeenOn.Value.ToString("MMMM d, yyyy - hh\\:mm",
                                CultureInfo.CreateSpecificCulture("en-US"))
                            : ""));
        }
    }
}