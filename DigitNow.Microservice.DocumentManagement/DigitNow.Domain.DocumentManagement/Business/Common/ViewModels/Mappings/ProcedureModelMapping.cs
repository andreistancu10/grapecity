using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings.Common;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class ProcedureModelMapping: Profile
    {
        public ProcedureModelMapping()
        {
            CreateMap<ProcedureAggregate, ProcedureViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Procedure.Id))
                .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Procedure.Title))
                .ForMember(x => x.Edition, opt => opt.MapFrom(src => src.Procedure.Edition))
                .ForMember(x => x.Revision, opt => opt.MapFrom(src => src.Procedure.Revision))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom<CommonMapUser>())
                .ForMember(x => x.StartDate, opt => opt.MapFrom(src => src.Procedure.StartDate))
                .ForMember(x => x.ProcedureCategory, opt => opt.MapFrom(src => src.Procedure.ProcedureCategory))
                .ForMember(x => x.Code, opt => opt.MapFrom(src => src.Procedure.Code))
                .ForMember(x => x.StateId, opt => opt.MapFrom(src => src.Procedure.StateId))
                .ForMember(x => x.Department, opt => opt.MapFrom<MapDepartment>())
                .ForMember(x => x.SpecificObjective, opt => opt.MapFrom<MapProcedureSpecificObjective>());
        }

        public class MapDepartment : IValueResolver<ProcedureAggregate, ProcedureViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(ProcedureAggregate source, ProcedureViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                var foundDepartment = source.Departments.FirstOrDefault(c => c.Id == source.Procedure.DepartmentId);

                return foundDepartment == null
                    ? null
                    : new BasicViewModel(foundDepartment.Id, foundDepartment.Name);
            }
        }
        public class MapProcedureSpecificObjective : IValueResolver<ProcedureAggregate, ProcedureViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(ProcedureAggregate source, ProcedureViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                var foundSpecificObjective = source.SpecificObjectives.FirstOrDefault(c => c.ObjectiveId == source.Procedure.SpecificObjectiveId);
                return foundSpecificObjective == null
                    ? null
                    : new BasicViewModel(foundSpecificObjective.Id, foundSpecificObjective.Title);
            }
        }
    }
}
