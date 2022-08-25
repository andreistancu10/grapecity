using System;

namespace DigitNow.Domain.DocumentManagement.Contracts.Interfaces.UploadedFiles
{
    public interface IUploadedFile
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string RelativePath { get; set; }
        public string AbsolutePath { get; set; }
        public Guid Guid { get; set; }
    }
}
