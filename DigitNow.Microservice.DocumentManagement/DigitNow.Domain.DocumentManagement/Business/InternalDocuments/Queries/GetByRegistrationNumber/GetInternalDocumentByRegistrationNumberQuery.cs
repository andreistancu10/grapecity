using System.Collections.Generic;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Queries.GetByRegistrationNumber;

public record GetInternalDocumentByRegistrationNumberQuery : IQuery<List<GetInternalDocumentByRegistrationNumberResponse>>
{
    public int RegistrationNumber { get; init; }
}