using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments.Queries;

public class OutgoingDocumentsQueryService : IOutgoingDocumentsQueryService
{
    protected readonly DocumentManagementDbContext _dbContext;

    public OutgoingDocumentsQueryService(DocumentManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<OutgoingDocument>> GetDocsByRegistrationNumber(string registrationNumber, CancellationToken cancellationToken)
    {
        return await _dbContext.OutgoingDocuments
            .Where(doc => doc.RegistrationNumber == registrationNumber).ToListAsync(cancellationToken);
    }
}