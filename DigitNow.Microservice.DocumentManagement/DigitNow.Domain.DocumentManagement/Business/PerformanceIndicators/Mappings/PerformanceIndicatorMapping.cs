using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.PerformanceIndicators.Commands;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.PerformanceIndicators.Mappings
{
    public class PerformanceIndicatorMapping: Profile
    {
        public PerformanceIndicatorMapping()
        {
            CreateMap<UpdatePerformanceIndicatorCommand, PerformanceIndicator>();
        }
    }
}
