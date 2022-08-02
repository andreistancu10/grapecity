using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface ISpecialRegisterMappingService
    {
        Task MapDocumentAsync(IncomingDocument incomingDocument, CancellationToken cancellationToken);
    }

    public class SpecialRegisterMappingService : ISpecialRegisterMappingService
    {
        private readonly DocumentManagementDbContext _dbContext;

        public SpecialRegisterMappingService(
            DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task MapDocumentAsync(IncomingDocument incomingDocument, CancellationToken cancellationToken)
        {
            var register = await GetDocumentSpecialRegisterAsync(incomingDocument.DocumentTypeId, cancellationToken);

            if (register != null)
            {
                await AddDocumentMappingAsync(incomingDocument.DocumentId, register, cancellationToken);
            }
        }

        private async Task AddDocumentMappingAsync(long documentId, SpecialRegister register,
            CancellationToken cancellationToken)
        {
            var newMapping = new SpecialRegisterMapping
            {
                DocumentId = documentId,
                SpecialRegister = register
            };

            await _dbContext.SpecialRegisterMappings.AddAsync(newMapping, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task<SpecialRegister> GetDocumentSpecialRegisterAsync(int documentType,
            CancellationToken cancellationToken)
        {
            return await _dbContext.SpecialRegisters
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.DocumentCategoryId == documentType, cancellationToken);
        }
    }
}