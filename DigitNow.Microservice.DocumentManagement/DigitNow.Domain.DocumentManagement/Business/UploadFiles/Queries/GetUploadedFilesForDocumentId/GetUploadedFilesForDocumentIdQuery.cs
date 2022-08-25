using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.GetUploadedFilesForDocumentId
{
    public class GetUploadedFilesForDocumentIdQuery : IQuery<List<DocumentFileViewModel>>
    {
        public long TargetId { get; set; }

        public GetUploadedFilesForDocumentIdQuery( long targetId)
        {
            TargetId = targetId;
        }
    }
}