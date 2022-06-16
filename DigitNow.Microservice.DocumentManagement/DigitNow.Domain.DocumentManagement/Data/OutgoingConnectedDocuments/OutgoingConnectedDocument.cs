namespace DigitNow.Domain.DocumentManagement.Data.OutgoingConnectedDocuments;

public class OutgoingConnectedDocument
{
    public int Id { get; set; }
    public long ChildOutgoingDocumentId { get; set; }
    public int RegistrationNumber { get; set; }
    public int DocumentType { get; set; }
}