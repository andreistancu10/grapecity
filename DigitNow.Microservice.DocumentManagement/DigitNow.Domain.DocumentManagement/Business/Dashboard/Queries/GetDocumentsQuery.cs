using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;

public class GetDocumentsQuery : IQuery<ResultPagedList<GetDocumentResponse>>
{
    public int Page { get; set; } = 1;
    public int Count { get; set; } = 10;
    public int UserId { get; set; } 
    public int UserRole { get; set; } 
    public int DepartmentId { get; set; } 
}