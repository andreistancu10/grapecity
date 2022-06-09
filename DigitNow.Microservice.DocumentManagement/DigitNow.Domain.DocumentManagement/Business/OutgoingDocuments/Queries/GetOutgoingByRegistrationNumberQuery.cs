using System.Collections.Generic;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries
{
    public class GetOutgoingByRegistrationNumberQuery : IQuery<List<GetDocsByRegistrationNumberResponse>>
    {
        public string RegistrationNumber { get; set; }
    }
}
