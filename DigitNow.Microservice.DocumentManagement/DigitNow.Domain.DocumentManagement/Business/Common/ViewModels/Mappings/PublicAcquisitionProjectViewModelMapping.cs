using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class PublicAcquisitionProjectViewModelMapping : Profile
    {
        public PublicAcquisitionProjectViewModelMapping()
        {
            CreateMap<PublicAcquisitionProject, GetPublicAcquisitionProjectViewModel>();

            CreateMap<PublicAcquisitionAggregate, PublicAcquisitionViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.PublicAcquisitionProject.Id))
                .ForMember(x => x.Code, opt => opt.MapFrom(src => src.PublicAcquisitionProject.Code))
                .ForMember(x => x.Type, opt => opt.MapFrom(src => src.PublicAcquisitionProject.Type))
                .ForMember(x => x.EstablishedProcedure, opt => opt.MapFrom<MapEstablishedProcedure>())
                .ForMember(x => x.CpvCode, opt => opt.MapFrom<MapCpvCode>());
        }

        private class MapEstablishedProcedure : IValueResolver<PublicAcquisitionAggregate, PublicAcquisitionViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(PublicAcquisitionAggregate source, PublicAcquisitionViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                var foundProcedure = source.EstablishedProcedures.FirstOrDefault(x => x.Id == source.PublicAcquisitionProject.EstablishedProcedure);
                if (foundProcedure != null)
                {
                    return new BasicViewModel(foundProcedure.Id, foundProcedure.Name);
                }
                return default;
            }
        }

        private class MapCpvCode : IValueResolver<PublicAcquisitionAggregate, PublicAcquisitionViewModel, BasicViewModel>
        {
            public BasicViewModel Resolve(PublicAcquisitionAggregate source, PublicAcquisitionViewModel destination, BasicViewModel destMember,
                ResolutionContext context)
            {
                var foundCpvCode = source.CpvCodes.FirstOrDefault(x => x.Id == source.PublicAcquisitionProject.CpvCode);
                if (foundCpvCode != null)
                {
                    return new BasicViewModel(foundCpvCode.Id, foundCpvCode.Name);
                }
                return default;
            }
        }
    }
}
