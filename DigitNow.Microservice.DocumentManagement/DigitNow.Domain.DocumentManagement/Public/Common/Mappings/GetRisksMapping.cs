using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Risks.Queries.GetRisks;
using DigitNow.Domain.DocumentManagement.Data.Filters.Risks;
using DigitNow.Domain.DocumentManagement.Public.Risks.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Common.Mappings
{
    public class GetRisksMapping : Profile
    {
        public GetRisksMapping()
        {
            CreateMap<GetRisksRequest, GetRisksQuery>()
                    .ForMember(x => x.Filter, opt => opt.MapFrom(src => src.Filter ?? new RiskFilterDto()));

            CreateMap<RiskFilterDto, RiskFilter>();
            {
                CreateMap<GeneralObjectiveFilterDto, GeneralObjectiveFilter>()
                    .ForMember(m => m.GeneralObjectiveId, opt => opt.MapFrom(src => src.GeneralObjectiveId));
                CreateMap<SpecificObjectiveFilterDto, SpecificObjectiveFilter>()
                    .ForMember(m => m.SpecificObjectiveId, opt => opt.MapFrom(src => src.SpecificObjectiveId));
                CreateMap<ActivityFilterDto, ActivityFilter>()
                    .ForMember(m => m.ActivityId, opt => opt.MapFrom(src => src.ActivityId));
                CreateMap<RiskNameFilterDto, RiskNameFilter>()
                    .ForMember(m => m.RiskName, opt => opt.MapFrom(src => src.RiskName));
                CreateMap<DepartmentFilterDto, DepartmentFilter>()
                    .ForMember(m => m.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId));
                CreateMap<DateOfLastRevisionFilterDto, DateOfLastRevisionFilter>()
                    .ForMember(m => m.DateOfLastRevision, opt => opt.MapFrom(src => src.DateOfLastRevision));
                CreateMap<ProbabilityOfApparitionFilterDto, ProbabilityOfApparitionFilter>()
                    .ForMember(m => m.ProbabilityOfApparitionEstimation, opt => opt.MapFrom(src => src.ProbabilityOfApparitionEstimation));
                CreateMap<ImpactOfObjectivesEstimationFilterDto, ImpactOfObjectivesEstimationFilter>()
                    .ForMember(m => m.ImpactOfObjectivesEstimation, opt => opt.MapFrom(src => src.ImpactOfObjectivesEstimation));
            }
        }
    }
}
