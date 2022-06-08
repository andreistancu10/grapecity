using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.Queries;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments.Queries;

public class OutgoingDocumentsQueryService : IOutgoingDocumentsQueryService
{
    protected readonly DocumentManagementDbContext _dbContext;

    public OutgoingDocumentsQueryService(DocumentManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IList<OutgoingDocument>> GetDocsById(long id, CancellationToken cancellationToken)
    {
        return await _dbContext.OutgoingDocuments
            .Where(doc => doc.Id == id).ToListAsync(cancellationToken);
    }
}