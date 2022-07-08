using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries.GetById
{
    public class GetIncomingDocumentByIdQuery : IQuery<GetIncomingDocumentByIdResponse>, IQuery<ResultPagedList<GetIncomingDocumentByIdResponse>>
    {
        public long Id { get; set; }
    }
}