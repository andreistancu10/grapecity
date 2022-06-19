using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;

public class GetDocumentsQuery : IQuery<ResultObject>
{
    public int Page { get; set; } = 1;
    public int Count { get; set; } = 10;
    public int UserId { get; set; } 
    public int UserRole { get; set; } 
    public int DepartmentId { get; set; } 
}