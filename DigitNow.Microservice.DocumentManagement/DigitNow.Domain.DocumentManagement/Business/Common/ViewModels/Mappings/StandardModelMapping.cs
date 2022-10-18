using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class StandardModelMapping: Profile
    {
        public StandardModelMapping()
        {
            CreateMap<StandardAggregate, StandardViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Standard.Id))
                .ForMember(x => x.Code, opt => opt.MapFrom(src => src.Standard.Code))
                .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Standard.Title))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => src.Standard.CreatedAt))
                .ForMember(x => x.StateId, opt => opt.MapFrom(src => src.Standard.StateId))
                .ForMember(x => x.Department, opt => opt.MapFrom<MapDepartment>());
        }
        public class MapDepartment : IValueResolver<StandardAggregate, StandardViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(StandardAggregate source, StandardViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                var foundDepartment = source.Departments.FirstOrDefault(c => c.Id == source.Standard.DepartmentId);

                return foundDepartment == null
                    ? null
                    : new BasicViewModel(foundDepartment.Id, foundDepartment.Name);
            }
        }
    }
}
