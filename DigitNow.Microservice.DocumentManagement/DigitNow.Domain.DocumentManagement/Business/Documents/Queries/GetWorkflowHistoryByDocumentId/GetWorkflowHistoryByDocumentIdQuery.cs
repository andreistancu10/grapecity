using HTSS.Platform.Core.CQRS;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetWorkflowHistoryByDocumentId
{
    public class GetWorkflowHistoryByDocumentIdQuery : IQuery<List<GetWorkflowHistoryByDocumentIdResponse>>
    {
        public long DocumentId { get; set; }
    }
}
