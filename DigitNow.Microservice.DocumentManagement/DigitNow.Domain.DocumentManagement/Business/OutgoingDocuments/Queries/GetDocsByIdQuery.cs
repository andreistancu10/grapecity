using System.Collections.Generic;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries
{
    public class GetDocsByIdQuery : IQuery<List<GetDocsByIdResponse>>
    {
        public long Id { get; set; }
    }
}
