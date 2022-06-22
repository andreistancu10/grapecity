using System;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetByRegistrationNumberAndYear;

public class GetDocumentsByRegistrationNumberAndYearResponse
{
    public int RegistrationNumber { get; set; }
    public int Id { get; set; }
    public int DocumentType { get; set; }
}