namespace DigitNow.Domain.DocumentManagement.Public.InternalDocuments.Models;

public record CreateInternalDocumentRequest(
    int RegistrationNumber,
    int DepartmentId,
    int InternalDocumentTypeId,
    int DeadlineDaysNumber,
    string Description,
    string? Observation,
    int ReceiverDepartmentId,
    bool? IsUrgent);