using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Documents.Commands.CreateDocumentDeliveryDetails;
using DigitNow.Domain.DocumentManagement.Public.Documents.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Documents.Mappings
{
    public class CreateDeliveryDetailsMapping : Profile
    {
        public CreateDeliveryDetailsMapping()
        {
            CreateMap<CreateDeliveryDetailsRequest, CreateDocumentDeliveryDetailsCommand>();
        }
    }
}
