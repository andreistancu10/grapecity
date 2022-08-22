using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using Microsoft.AspNetCore.Http;

namespace DigitNow.Domain.DocumentManagement.Public.UploadFiles.Models
{
    public class UploadFileRequest
    {
        public string UsageContext { get; set; }
        public UsageLocation UsageLocation { get; set; }
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}