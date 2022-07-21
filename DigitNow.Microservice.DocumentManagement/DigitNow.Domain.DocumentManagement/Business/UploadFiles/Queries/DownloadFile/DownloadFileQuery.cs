using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.DownloadFile
{
    public class DownloadFileQuery : IQuery<DownloadFileResponse>
    {
        public long FileId { get; set; }

        public DownloadFileQuery(long fileId)
        {
            FileId = fileId;
        }
    }
}