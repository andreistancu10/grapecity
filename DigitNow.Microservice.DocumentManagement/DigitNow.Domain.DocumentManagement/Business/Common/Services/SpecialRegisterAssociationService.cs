using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.DocumentSpecialRegisters;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisters;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services;

public class SpecialRegisterAssociationService : ISpecialRegisterAssociationService
{
    private readonly DocumentManagementDbContext _dbContext;

    public SpecialRegisterAssociationService(
        DocumentManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> AssociateDocumentAsync(IncomingDocument document)
    {
        if (!CheckIfRegisterExistsForDocument(document.DocumentTypeId, out var register))
        {
            return false;
        }

        await CreateDocumentAssociationAsync(document, register);
        return true;
    }

    private async Task CreateDocumentAssociationAsync(IncomingDocument document, SpecialRegister register)
    {
        var newAssociation = new SpecialRegisterAssociation
        {
            Document = document,
            SpecialRegister = register
        };

        await _dbContext.SpecialRegisterAssociations.AddAsync(newAssociation);
        await _dbContext.SaveChangesAsync();
    }

    private bool CheckIfRegisterExistsForDocument(int documentType, out SpecialRegister register)
    {
        register = _dbContext.SpecialRegisters.AsNoTracking().FirstOrDefaultAsync(c => c.DocumentCategoryId == documentType).Result;

        return register is { };
    }
}

public interface ISpecialRegisterAssociationService
{
    Task<bool> AssociateDocumentAsync(IncomingDocument document);
}