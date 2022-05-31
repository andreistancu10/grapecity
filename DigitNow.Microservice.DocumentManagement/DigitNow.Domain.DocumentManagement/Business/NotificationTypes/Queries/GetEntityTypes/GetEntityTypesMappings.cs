using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Helpers;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Queries.GetEntityTypes
{
    public class GetEntityTypesMappings : Profile
    {
        public GetEntityTypesMappings()
        {
            CreateMap<NotificationEntityType, GetEntityTypesResponse>();
        }
    }
}
