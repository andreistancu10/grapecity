
namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries;

public class GetDocsByRegistrationNumberResponse
{
    public int Id { get; set; }

    public int RegistrationNumber { get; set; }
    
    public int DocumentType { get; set; }
}