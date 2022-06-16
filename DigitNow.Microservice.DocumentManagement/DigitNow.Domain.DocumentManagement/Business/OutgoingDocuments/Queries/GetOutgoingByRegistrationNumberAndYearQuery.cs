using System.Collections.Generic;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries;

public class GetOutgoingByRegistrationNumberAndYearQuery : IQuery<List<GetDocsByRegistrationNumberResponse>>
{
    public int RegistrationNumber { get; set; }
    public int Year { get; set; }
}