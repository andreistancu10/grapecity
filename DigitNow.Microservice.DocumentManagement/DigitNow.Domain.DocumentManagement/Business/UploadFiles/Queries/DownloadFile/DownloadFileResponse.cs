using HTSS.Platform.Core.Files.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.DownloadFile
{
    public class DownloadFileResponse
    {
        public DownloadFileResponse(FileContent fileContent)
        {
            FileContent = fileContent;
        }

        public FileContent FileContent{ get; set; }
    }
}