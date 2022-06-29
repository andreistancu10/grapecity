namespace DigitNow.Domain.DocumentManagement.Public.InternalDocuments.Models;

public record CreateInternalDocumentRequest(
    int DepartmentId,
    int InternalDocumentTypeId,  //TODO: Rename it to DocumentCategoryId
    int DeadlineDaysNumber,
    string Description,
    string? Observation,
    int ReceiverDepartmentId,
    bool? IsUrgent);