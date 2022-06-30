using DigitNow.Domain.DocumentManagement.Data.Entities.UploadedFiles;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.DocumentUploadedFiles;

public class DocumentUploadedFile : ExtendedEntity
{
    public Document Document { get; set; }
    public UploadedFile UploadedFile { get; set; }
}