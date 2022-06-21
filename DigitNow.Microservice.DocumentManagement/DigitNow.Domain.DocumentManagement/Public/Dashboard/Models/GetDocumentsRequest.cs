namespace DigitNow.Domain.DocumentManagement.Public.Dashboard.Models;

public class GetDocumentsRequest
{
    public int Page { get; set; } = 1;
    public int Count { get; set; } = 10;
}