using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments.Queries;

public interface IOutgoingDocumentsQueryService
{
    Task<IList<OutgoingDocument>> GetDocsByRegistrationNumber(int registrationNumber,
        CancellationToken cancellationToken);
}