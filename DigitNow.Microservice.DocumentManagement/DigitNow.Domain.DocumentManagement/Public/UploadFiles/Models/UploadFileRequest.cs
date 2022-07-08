using Microsoft.AspNetCore.Http;

namespace DigitNow.Domain.DocumentManagement.Public.UploadFiles.Models;

public class UploadFileRequest
{
    public long DocumentCategoryId { get; set; }
    public string Name { get; set; }
    public IFormFile File { get; set; }
}