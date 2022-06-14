using System;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Queries.GetByRegistrationNumber;

public record GetInternalDocumentByRegistrationNumberResponse(
    int Id,
    int RegistrationNumber,
    DateTime RegistrationDate,
    int DepartmentId,
    int InternalDocumentTypeId,
    int DeadlineDaysNumber,
    string Description,
    string? Observation,
    int ReceiverDepartmentId,
    bool? IsUrgent,
    string User);