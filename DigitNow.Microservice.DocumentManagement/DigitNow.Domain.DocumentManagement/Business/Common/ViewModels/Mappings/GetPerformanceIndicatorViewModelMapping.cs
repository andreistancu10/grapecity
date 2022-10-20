using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class GetPerformanceIndicatorViewModelMapping: Profile
    {
        public GetPerformanceIndicatorViewModelMapping()
        {
            CreateMap<PerformanceIndicator, GetPerformanceIndicatorViewModel>()
                .ForPath(c => c.FunctionaryIds, opy => opy.MapFrom(src => src.PerformanceIndicatorFunctionaries.Select(x => x.FunctionaryId).ToList()));
        }
    }
}
