using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetWorkflowInformation
{
    public class GetWorkflowInformationByDocumentIdQuery : IQuery<GetWorkflowInformationByDocumentIdResponse>
    {
        public long DocumentId { get; set; }
    }
}
