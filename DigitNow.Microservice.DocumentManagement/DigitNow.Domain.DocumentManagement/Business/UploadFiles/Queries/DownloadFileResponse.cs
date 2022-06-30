using System;
using System.IO;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries;

public class DownloadFileResponse
{
    public int Id { get; set; }
    public Guid Guid{ get; set; }
    public string Name { get; set; }
    public string RelativePath { get; set; }
    public Stream FileStream { get; set; }
}