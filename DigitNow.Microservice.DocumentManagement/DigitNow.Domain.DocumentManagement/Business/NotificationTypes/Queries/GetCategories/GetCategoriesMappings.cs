using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Helpers;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Queries.GetCategories
{
    public class GetCategoriesMappings : Profile
    {
        public GetCategoriesMappings()
        {
            CreateMap<NotificationTypeCategory, GetCategoriesResponse>();
        }
    }
}
