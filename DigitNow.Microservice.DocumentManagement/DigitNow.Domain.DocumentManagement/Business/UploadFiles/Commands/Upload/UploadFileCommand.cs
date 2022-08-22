using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using HTSS.Platform.Core.CQRS;
using Microsoft.AspNetCore.Http;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload
{
    public class UploadFileCommand : ICommand<DocumentFileViewModel>
    {
        public object UsageContext { get; set; }
        public UsageLocation UsageLocation { get; set; }
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}