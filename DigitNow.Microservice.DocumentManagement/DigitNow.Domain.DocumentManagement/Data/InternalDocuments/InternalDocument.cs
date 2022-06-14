using System;

namespace DigitNow.Domain.DocumentManagement.Data.InternalDocuments;

public class InternalDocument
{
    public int Id { get; set; }
    public int RegistrationNumber { get; set; }
    public DateTime RegistrationDate { get; set; }
    public int DepartmentId { get; set; }
    public int InternalDocumentTypeId { get; set; }
    public int DeadlineDaysNumber { get; set; }
    public string Description { get; set; }
    public string Observation { get; set; }
    public int ReceiverDepartmentId { get; set; }
    public bool? IsUrgent { get; set; }
    public string User { get; set; }
}