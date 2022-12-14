namespace DigitNow.Domain.DocumentManagement.Public.InternalDocuments.Models;

public record CreateInternalDocumentRequest(
    int SourceDepartmentId,
    int InternalDocumentTypeId,  //TODO: Rename it to DocumentCategoryId
    int DeadlineDaysNumber,
    string Description,
    string? Observation,
    int DestinationDepartmentId,
    bool? IsUrgent,
    List<long> UploadedFileIds);