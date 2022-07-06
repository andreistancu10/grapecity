using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetById
{
    public class GetOutgoingDocumentByIdQuery : IQuery<GetOutgoingDocumentByIdResponse>, IQuery<ResultPagedList<GetOutgoingDocumentByIdResponse>>
    {
        public long Id { get; set; }
    }
}