using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class RiskModelMapping : Profile
    {
        public RiskModelMapping()
        {
            CreateMap<RiskAggregate, RiskViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Risk.Id))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Risk.Description))
                .ForMember(x => x.Code, opt => opt.MapFrom(src => src.Risk.Code))
                .ForMember(x => x.SpecificObjective, opt => opt.MapFrom<MapRiskSpecificObjective>())
                .ForMember(x => x.DateOfLastRevision, opt => opt.MapFrom(src => src.Risk.ModifiedAt ?? src.Risk.CreatedAt))
                .ForMember(x => x.State, opt => opt.MapFrom(src => src.Risk.State))
                .ForMember(x => x.Department, opt => opt.MapFrom<MapDepartment>());
        }
    }

    public class MapRiskSpecificObjective : IValueResolver<RiskAggregate, RiskViewModel, BasicViewModel>
    {
        public BasicViewModel Resolve(RiskAggregate source, RiskViewModel destination, BasicViewModel destMember,
            ResolutionContext context)
        {
            var foundSpecificObjective = source.SpecificObjectives.FirstOrDefault(c => c.ObjectiveId == source.Risk.SpecificObjectiveId);
            return foundSpecificObjective == null
                ? null
                : new BasicViewModel(foundSpecificObjective.Id, foundSpecificObjective.Title);
        }
    }

    public class MapDepartment : IValueResolver<RiskAggregate, RiskViewModel, BasicViewModel>
    {
        public BasicViewModel Resolve(RiskAggregate source, RiskViewModel destination, BasicViewModel destMember,
            ResolutionContext context)
        {
            var foundDepartment = source.Departments.FirstOrDefault(c => c.Id == source.Risk.DepartmentId);

            return foundDepartment == null
                ? null
                : new BasicViewModel(foundDepartment.Id, foundDepartment.Name);
        }
    }

}
