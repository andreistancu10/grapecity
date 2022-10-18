using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.PublicAcquisitions.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.PublicAcquisitions.Commands.Update;
using DigitNow.Domain.DocumentManagement.Public.PublicAcquisitions.Models;

namespace DigitNow.Domain.DocumentManagement.Public.PublicAcquisitions.Mappings
{
    public class PublicAcquisitionMappings : Profile
    {
        public PublicAcquisitionMappings()
        {
            CreateMap<CreatePublicAcquisitionProjectRequest, CreatePublicAcquisitionProjectCommand>();
            CreateMap<UpdatePublicAcquisitionProjectRequest, UpdatePublicAcquisitionProjectCommand>();
        }
    }
}
