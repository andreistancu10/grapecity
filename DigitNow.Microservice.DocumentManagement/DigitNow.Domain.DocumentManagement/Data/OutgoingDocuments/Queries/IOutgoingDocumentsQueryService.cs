using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;

namespace DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments.Queries
{
    public interface IOutgoingDocumentsQueryService
    {
        Task<IList<OutgoingDocument>> GetDocsById(long id, CancellationToken cancellationToken);
    }
}
