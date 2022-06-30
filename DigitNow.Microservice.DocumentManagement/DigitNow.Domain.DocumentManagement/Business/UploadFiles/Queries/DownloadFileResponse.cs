using HTSS.Platform.Core.Files.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries;

public class DownloadFileResponse
{
    public DownloadFileResponse(FileContent fileContent)
    {
        FileContent = fileContent;
    }

    public FileContent FileContent{ get; set; }
}