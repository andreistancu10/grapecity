using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.DocumentSpecialRegisters;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
using DigitNow.Domain.DocumentManagement.Data.SpecialRegisters;
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
        if (!CheckDocumentTypeSpecificRegister(document.DocumentTypeId, out var register))
        {
            return false;
        }

        await CreateDocumentAssociationAsync(document, register);
        return true;
    }

    private async Task CreateDocumentAssociationAsync(IncomingDocument document, SpecialRegister register)
    {
        var newAssociation = new DocumentSpecialRegister
        {
            Document = document,
            SpecialRegister = register
        };

        await _dbContext.DocumentSpecialRegisterAssociations.AddAsync(newAssociation);
        await _dbContext.SaveChangesAsync();
    }

    private bool CheckDocumentTypeSpecificRegister(int documentType, out SpecialRegister register)
    {
        register = _dbContext.SpecialRegisters.AsNoTracking().FirstOrDefaultAsync(c => c.DocumentType == documentType).Result;

        return register is { };
    }
}

public interface ISpecialRegisterAssociationService
{
    Task<bool> AssociateDocumentAsync(IncomingDocument document);
}