using HTSS.Platform.Core.Files.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.GetFiles
{
    public class GetFilesResponse
    {
        public GetFilesResponse(FileContent fileContent)
        {
            FileContent = fileContent;
        }

        public FileContent FileContent{ get; set; }
    }
}