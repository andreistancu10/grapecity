using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisterMapping;
using DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisters;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services;

public interface ISpecialRegisterMappingService
{
    Task<bool> MapDocumentAsync(IncomingDocument incomingDocument, CancellationToken cancellationToken);
}

public class SpecialRegisterMappingService : ISpecialRegisterMappingService
{
    private readonly DocumentManagementDbContext _dbContext;

    public SpecialRegisterMappingService(
        DocumentManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> MapDocumentAsync(IncomingDocument incomingDocument, CancellationToken cancellationToken)
    {
        var register = await FindDocumentSpecialRegisterAsync(incomingDocument.DocumentTypeId, cancellationToken);
        await AddDocumentMappingAsync(incomingDocument, register, cancellationToken);

        return true;
    }

    private async Task AddDocumentMappingAsync(IncomingDocument incomingDocument, SpecialRegister register,
        CancellationToken cancellationToken)
    {
        var newMapping = new SpecialRegisterMapping
        {
            IncomingDocument = incomingDocument,
            SpecialRegister = register
        };

        await _dbContext.SpecialRegisterMappings.AddAsync(newMapping, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<SpecialRegister> FindDocumentSpecialRegisterAsync(int documentType,
        CancellationToken cancellationToken)
    {
        return await _dbContext.SpecialRegisters.AsNoTracking().FirstAsync(c => c.DocumentCategoryId == documentType, cancellationToken);
    }
}