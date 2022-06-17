using System.Collections.Generic;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetByRegistrationNumberAndYear;

public record GetOutgoingDocumentsByRegistrationNumberAndYearQuery : IQuery<List<GetDocumentsByRegistrationNumberAndYearResponse>>
{
    public int RegistrationNumber { get; init; }
    public int Year { get; init; }
}