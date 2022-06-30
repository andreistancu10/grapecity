using System;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.UploadedFiles;

public class UploadedFile : ExtendedEntity
{
    public string Name { get; set; }
    public string RelativePath { get; set; }
    public Guid Guid { get; set; }
}