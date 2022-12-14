using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class SpecialRegisterViewModelMappings : Profile
    {
        public SpecialRegisterViewModelMappings()
        {
            CreateMap<SpecialRegisterViewModelAggregate, SpecialRegisterViewModel>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src=>src.SpecialRegister.Id))
                .ForMember(c => c.Name, opt => opt.MapFrom(src=>src.SpecialRegister.Name))
                .ForMember(c => c.Observations, opt => opt.MapFrom(src=>src.SpecialRegister.Observations))
                .ForMember(c => c.DocumentCategory, opt => opt.MapFrom<MapDocumentCategory>());

            CreateMap<SpecialRegisterViewModel, SpecialRegisterExportViewModel>()
                .ForMember(c => c.DocumentCategory, opt => opt.MapFrom(src => src.DocumentCategory.Name));
        }

        public class MapDocumentCategory : IValueResolver<SpecialRegisterViewModelAggregate, SpecialRegisterViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(SpecialRegisterViewModelAggregate source, SpecialRegisterViewModel destination, BasicViewModel destMember, ResolutionContext context)
            {
                var found = source.Categories.FirstOrDefault(c => c.Id == source.SpecialRegister.DocumentCategoryId);

                if (found == null)
                {
                    return null;
                }

                return new BasicViewModel(found.Id, found.Name);
            }
        }
    }
}