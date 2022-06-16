using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Commands.Update;
using DigitNow.Domain.DocumentManagement.Public.Dashboard.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Dashboard.Mappings
{
    public class UpdateDocumentRecipientMapping : Profile
    {
        public UpdateDocumentRecipientMapping()
        {
            CreateMap<UpdateDocumentRecipientRequest, UpdateDocumentRecipientCommand>();
        }
    }
}
