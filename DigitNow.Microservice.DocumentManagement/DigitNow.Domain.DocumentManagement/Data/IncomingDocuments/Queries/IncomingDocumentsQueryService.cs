using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.Queries
{
    public interface IDocumentsQueryService
    {
        Task<IList<IncomingDocument>> GetDocsByRegistrationNumber(string registrationNumber, CancellationToken cancellationToken);
    }

    public class IncomingDocumentsQueryService : IDocumentsQueryService
    {
        protected readonly DocumentManagementDbContext _dbContext;

        public IncomingDocumentsQueryService(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IList<IncomingDocument>> GetDocsByRegistrationNumber(string registrationNumber, CancellationToken cancellationToken)
        {
            return await _dbContext.IncomingDocuments
                .Where(doc => doc.RegistrationNumber.StartsWith(registrationNumber)).ToListAsync(cancellationToken);
        }
    }
}
