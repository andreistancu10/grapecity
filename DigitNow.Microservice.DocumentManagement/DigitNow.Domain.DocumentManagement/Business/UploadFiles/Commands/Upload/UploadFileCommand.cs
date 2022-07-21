using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;
using Microsoft.AspNetCore.Http;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload
{
    public class UploadFileCommand : ICommand<FileViewModel>
    {
        public long? DocumentId { get; set; }
        public long DocumentCategoryId { get; set; }
        public string  Name { get; set; }
        public IFormFile File{ get; set; }
    }
}