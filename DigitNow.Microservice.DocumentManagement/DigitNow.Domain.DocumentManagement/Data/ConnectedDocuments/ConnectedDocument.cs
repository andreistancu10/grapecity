﻿namespace DigitNow.Domain.DocumentManagement.Data.ConnectedDocuments;

public class ConnectedDocument
{
    public int Id { get; set; }
    public int ChildDocumentId { get; set; }
    public int RegistrationNumber { get; set; }
    public int DocumentType { get; set; }
}