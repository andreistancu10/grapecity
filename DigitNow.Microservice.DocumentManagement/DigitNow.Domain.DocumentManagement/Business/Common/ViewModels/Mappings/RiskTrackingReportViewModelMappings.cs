using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Data.Entities.Risks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class RiskTrackingReportViewModelMappings : Profile
    {
        public RiskTrackingReportViewModelMappings()
        {
            CreateMap<RiskTrackingReportAggregate, RiskTrackingReportViewModel>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom<MapRiskTrackingReportUser>())
                .ForMember(dest => dest.ControlMeasuresImplementationState, opt => opt.MapFrom(src => src.RiskTrackingReport.ControlMeasuresImplementationState))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.RiskTrackingReport.CreatedAt))
                .ForMember(dest => dest.RiskExposureEvaluation, opt => opt.MapFrom(src => src.RiskTrackingReport.RiskExposureEvaluation))
                .ForMember(dest => dest.Difficulties, opt => opt.MapFrom(src => src.RiskTrackingReport.Difficulties))
                .ForMember(dest => dest.ImpactOfObjectivesEstimation, opt => opt.MapFrom(src => src.RiskTrackingReport.ImpactOfObjectivesEstimation))
                .ForMember(dest => dest.ProbabilityOfApparitionEstimation, opt => opt.MapFrom(src => src.RiskTrackingReport.ProbabilityOfApparitionEstimation))
                .ForMember(dest => dest.RiskActionProposals, opt => opt.MapFrom(src => src.RiskTrackingReport.RiskActionProposals));

            CreateMap<RiskActionProposal, RiskActionProposalViewModel>();
        }

        private class MapRiskTrackingReportUser : IValueResolver<RiskTrackingReportAggregate, RiskTrackingReportViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(RiskTrackingReportAggregate source, RiskTrackingReportViewModel destination, BasicViewModel destMember, ResolutionContext context) =>
                ExtractUser(source);

            private static BasicViewModel ExtractUser(RiskTrackingReportAggregate source)
            {
                var foundUser = source.Users.FirstOrDefault(x => x.Id == source.RiskTrackingReport.CreatedBy);
                if (foundUser != null)
                {
                    return new BasicViewModel(foundUser.Id, $"{foundUser.FirstName} {foundUser.LastName}");
                }
                return default;
            }
        }
    }
}
