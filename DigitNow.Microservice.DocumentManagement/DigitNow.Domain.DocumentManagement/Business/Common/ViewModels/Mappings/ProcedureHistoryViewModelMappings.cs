using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings.Common;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class ProcedureHistoryViewModelMappings : Profile
    {
        public ProcedureHistoryViewModelMappings()
        {
            CreateMap<ProcedureHistoryAggregate, ProcedureHistoryViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProcedureHistory.Id))
                .ForMember(dest => dest.Revision, opt => opt.MapFrom(src => src.ProcedureHistory.Id))
                .ForMember(dest => dest.Edition, opt => opt.MapFrom(src => src.ProcedureHistory.Id))
                .ForMember(dest => dest.Procedure, opt => opt.MapFrom<MapProcedure>())
                .ForMember(dest => dest.User, opt => opt.MapFrom<CommonMapUser>());
        }

        public class MapProcedure : IValueResolver<ProcedureHistoryAggregate, ProcedureHistoryViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(ProcedureHistoryAggregate source, ProcedureHistoryViewModel destination,
                BasicViewModel destMember, ResolutionContext context)
            {
                return new BasicViewModel(source.Procedure.Id, source.Procedure.Title);
            }
        }
    }
}