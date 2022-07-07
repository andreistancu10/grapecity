using HTSS.Platform.Core.CQRS;
using Microsoft.AspNetCore.Http;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload;

public class UploadFileCommand : ICommand<ResultObject>
{
    public long DocumentCategoryId { get; set; }
    public string  Name { get; set; }
    public IFormFile File{ get; set; }
}