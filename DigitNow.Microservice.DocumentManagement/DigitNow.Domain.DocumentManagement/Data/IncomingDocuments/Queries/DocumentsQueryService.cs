using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.Queries
{
    public interface IDocumentsQueryService
    {
        Task<IList<IncomingDocument>> GetIncomingDocsByRegistrationNumberAndYear(int registrationNumber, int year, CancellationToken cancellationToken);

        Task<IList<InternalDocument.InternalDocument>> GetInternalDocsByRegistrationNumberAndYear(
            int registrationNumber, int year, CancellationToken cancellationToken);
    }
    public class DocumentsQueryService : IDocumentsQueryService
    {
        private readonly DocumentManagementDbContext _dbContext;

        public DocumentsQueryService(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<IList<IncomingDocument>> GetIncomingDocsByRegistrationNumberAndYear(int registrationNumber, int year, CancellationToken cancellationToken)
        {
            return await _dbContext.IncomingDocuments
                .Where(doc => doc.RegistrationNumber == registrationNumber && doc.CreationDate.Year == year).ToListAsync(cancellationToken);
        }        
        
        public async Task<IList<InternalDocument.InternalDocument>> GetInternalDocsByRegistrationNumberAndYear(int registrationNumber, int year, CancellationToken cancellationToken)
        {
            return await _dbContext.InternalDocuments
                .Where(doc => doc.RegistrationNumber == registrationNumber && doc.RegistrationDate.Year == year).ToListAsync(cancellationToken);
        }
    }
}
