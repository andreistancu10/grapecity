using DigitNow.Domain.DocumentManagement.Data.IncomingConnectedDocuments;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.Queries;

public interface IDocumentsQueryService
{
    Task<IncomingConnectedDocument> GetDocsByRegistrationNumberAndYear(int registrationNumber, int year, CancellationToken cancellationToken);
    Task<IncomingDocument> GetIncomingDocumentByRegistrationNumber(int registrationNumber);
}

public class DocumentsQueryService : IDocumentsQueryService
{
    protected readonly DocumentManagementDbContext _dbContext;

    public DocumentsQueryService(DocumentManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IncomingDocument> GetIncomingDocumentByRegistrationNumber(int registrationNumber)
    {
        return await _dbContext.IncomingDocuments
                               .Include(doc => doc.WorkflowHistory)
                               .FirstOrDefaultAsync(doc => doc.RegistrationNumber == registrationNumber);
    }

    public async Task<IncomingConnectedDocument> GetDocsByRegistrationNumberAndYear(int registrationNumber, int year, CancellationToken cancellationToken)
    {
        var incomingDoc = await CheckIncomingDocumentsByRegistrationNumber(registrationNumber, year);
            
        if (incomingDoc != null)
        {
            return incomingDoc;
        }

        var internalDoc = await CheckInternalDocumentsByRegistrationNumber(registrationNumber, year);

        return internalDoc;
    }

    private async Task<IncomingConnectedDocument> CheckInternalDocumentsByRegistrationNumber(int registrationNumber, int year)
    {
        var internalDoc = await _dbContext.InternalDocuments
            .FirstOrDefaultAsync(doc => doc.RegistrationNumber == registrationNumber && doc.RegistrationDate.Year == year);

        if (internalDoc != null)
        {
            return new IncomingConnectedDocument { RegistrationNumber = internalDoc.RegistrationNumber, Id = internalDoc.Id, DocumentType = internalDoc.InternalDocumentTypeId };
        }

        return null;
    }
    
    private async Task<IncomingConnectedDocument> CheckIncomingDocumentsByRegistrationNumber(int registrationNumber, int year)
    {
        var incomingDoc = await _dbContext.IncomingDocuments
            .FirstOrDefaultAsync(doc => doc.RegistrationNumber == registrationNumber && doc.CreationDate.Year == year);

        if (incomingDoc != null)
        {
            return new IncomingConnectedDocument() { RegistrationNumber = incomingDoc.RegistrationNumber, Id = incomingDoc.Id, DocumentType = incomingDoc.DocumentTypeId };
        }

        return null;
    }
}