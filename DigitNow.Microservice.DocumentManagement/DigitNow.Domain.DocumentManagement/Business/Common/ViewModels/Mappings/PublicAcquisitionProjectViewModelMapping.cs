using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class PublicAcquisitionProjectViewModelMapping : Profile
    {
        public PublicAcquisitionProjectViewModelMapping()
        {
            CreateMap<PublicAcquisitionProject, GetPublicAcquisitionProjectViewModel>();
        }
    }
}
