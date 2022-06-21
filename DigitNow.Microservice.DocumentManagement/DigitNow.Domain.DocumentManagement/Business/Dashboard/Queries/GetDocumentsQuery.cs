using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;

public class GetDocumentsQuery : IQuery<ResultPagedList<GetDocumentResponse>>
{
    public int Page { get; set; } = 1;
    public int Count { get; set; } = 10;
    public string UserId { get; set; } 
}