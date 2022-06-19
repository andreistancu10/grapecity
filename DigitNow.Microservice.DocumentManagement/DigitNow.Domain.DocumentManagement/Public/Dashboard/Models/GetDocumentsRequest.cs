namespace DigitNow.Domain.DocumentManagement.Public.Dashboard.Models;

public class GetDocumentsRequest
{
    public int Page { get; set; } = 1;
    public int Count { get; set; } = 10;
    public int UserId { get; set; }
    public int UserRole { get; set; }
    public int DepartmentId { get; set; }
}