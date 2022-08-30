using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using Microsoft.AspNetCore.Http;

namespace DigitNow.Domain.DocumentManagement.Public.UploadFiles.Models
{
    public class UploadFileRequest
    {
        public IFormFile File { get; set; }
        public long? TargetId { get; set; }
        public long TargetEntity { get; set; }
        public string Context { get; set; }
    }
}