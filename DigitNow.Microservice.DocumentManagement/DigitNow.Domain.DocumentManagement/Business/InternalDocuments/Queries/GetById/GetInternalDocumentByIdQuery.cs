using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Queries.GetById
{
    public class GetInternalDocumentByIdQuery : IQuery<GetInternalDocumentByIdResponse>, IQuery<ResultPagedList<GetInternalDocumentByIdResponse>>
    {
        public long Id { get; set; }
    }
}