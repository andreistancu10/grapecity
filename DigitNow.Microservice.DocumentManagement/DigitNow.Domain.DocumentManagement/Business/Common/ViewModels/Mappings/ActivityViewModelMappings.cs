using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class ActivityViewModelMappings:Profile
    {
        public ActivityViewModelMappings()
        {
            CreateMap<Activity, ActivityViewModel>();
        }
    }
}