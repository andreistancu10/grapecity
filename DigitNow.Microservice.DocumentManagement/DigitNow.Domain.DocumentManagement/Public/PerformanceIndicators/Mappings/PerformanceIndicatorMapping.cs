using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.PerformanceIndicators.Commands;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Public.PerformanceIndicators.Models;

namespace DigitNow.Domain.DocumentManagement.Public.PerformanceIndicators.Mappings
{
    public class PerformanceIndicatorMapping: Profile
    {
        public PerformanceIndicatorMapping()
        {
            CreateMap<CreatePerformanceIndicatorRequest, CreatePerformanceIndicatorCommand>();
            CreateMap<UpdatePerformanceIndicatorRequest, UpdatePerformanceIndicatorCommand>();
            CreateMap<CreatePerformanceIndicatorCommand, PerformanceIndicator>();
        }
    }
}
