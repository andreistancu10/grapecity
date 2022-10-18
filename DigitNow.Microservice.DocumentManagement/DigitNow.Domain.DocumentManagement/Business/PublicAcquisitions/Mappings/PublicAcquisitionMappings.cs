using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.PublicAcquisitions.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.PublicAcquisitions.Commands.Update;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.PublicAcquisitions.Mappings
{
    public class PublicAcquisitionMappings : Profile
    {
        public PublicAcquisitionMappings()
        {
            CreateMap<CreatePublicAcquisitionProjectCommand, PublicAcquisitionProject>();
            CreateMap<UpdatePublicAcquisitionProjectCommand, PublicAcquisitionProject>();
        }
    }
}
