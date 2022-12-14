using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Commands.UpdateUserRecipient;
using DigitNow.Domain.DocumentManagement.Public.Dashboard.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Dashboard.Mappings
{
    public class UpdateDocumentUserRecipientMapping: Profile
    {
        public UpdateDocumentUserRecipientMapping()
        {
            CreateMap<UpdateDocumentUserRecipientRequest, UpdateDocumentUserRecipientCommand>();
        }
    }
}
