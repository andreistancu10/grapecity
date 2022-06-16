using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments.Queries;

public class OutgoingDocumentsQueryService : IOutgoingDocumentsQueryService
{
    protected readonly DocumentManagementDbContext DbContext;

    public OutgoingDocumentsQueryService(DocumentManagementDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<IList<OutgoingDocument>> GetDocsByRegistrationNumber(int registrationNumber, CancellationToken cancellationToken)
    {
        return await DbContext.OutgoingDocuments
            .Where(doc => doc.RegistrationNumber == registrationNumber).ToListAsync(cancellationToken);
    }
}