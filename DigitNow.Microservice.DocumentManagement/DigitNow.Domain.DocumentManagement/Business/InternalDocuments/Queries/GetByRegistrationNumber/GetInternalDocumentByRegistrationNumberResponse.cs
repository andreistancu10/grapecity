using System;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocument.Queries.GetByRegistrationNumber;

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