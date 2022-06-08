
using HTSS.Platform.Core.CQRS;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries
{
    public class GetDocsByRegistrationNumberQuery : IQuery<List<GetDocsByRegistrationNumberResponse>>
    {
        public int RegistrationNumber { get; set; }
    }
}
