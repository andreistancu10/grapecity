using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.ContactDetails.Queries
{
    public class GetContactDetailsByIdentificationNumberQuery : IQuery<GetContactDetailsByIdentificationNumberResponse>
    {
        public string IdentificationNumber { get; set; }
    }
}
