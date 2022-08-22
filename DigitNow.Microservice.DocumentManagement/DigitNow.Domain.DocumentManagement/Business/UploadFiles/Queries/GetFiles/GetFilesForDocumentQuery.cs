using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.GetFiles
{
    public class GetFilesForDocumentQuery : IQuery<List<DocumentFileViewModel>>
    {
        public long DocumentId { get; set; }

        public GetFilesForDocumentQuery(long documentId)
        {
            DocumentId = documentId;
        }
    }
}