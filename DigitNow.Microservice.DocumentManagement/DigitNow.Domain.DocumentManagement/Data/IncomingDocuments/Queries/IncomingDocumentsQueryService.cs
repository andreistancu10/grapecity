using DigitNow.Domain.DocumentManagement.Data.ConnectedDocuments;
using DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.Queries;

public interface IDocumentsQueryService
{
    Task<ConnectedDocument> GetDocsByRegistrationNumberAndYear(int registrationNumber, int year, CancellationToken cancellationToken);
    Task<IncomingDocument> GetIncomingDocumentById(int id, CancellationToken cancellationToken);
    Task<OutgoingDocument> GetOutgoingDocumentById(int id, CancellationToken cancellationToken);
}

public class DocumentsQueryService : IDocumentsQueryService
{
    protected readonly DocumentManagementDbContext _dbContext;

    public DocumentsQueryService(DocumentManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ConnectedDocument> GetDocsByRegistrationNumberAndYear(int registrationNumber, int year, CancellationToken cancellationToken)
    {
        var incomingDoc = await CheckIncomingDocumentsByRegistrationNumber(registrationNumber, year);
            
        if (incomingDoc != null)
        {
            return incomingDoc;
        }

        var internalDoc = await CheckInternalDocumentsByRegistrationNumber(registrationNumber, year);

        return internalDoc;
    }

    private async Task<ConnectedDocument> CheckInternalDocumentsByRegistrationNumber(int registrationNumber, int year)
    {
        var internalDoc = await _dbContext.InternalDocuments
            .FirstOrDefaultAsync(doc => doc.RegistrationNumber == registrationNumber && doc.CreationDate.Year == year);

        if (internalDoc != null)
        {
            return new ConnectedDocument { RegistrationNumber = internalDoc.RegistrationNumber, Id = internalDoc.Id, DocumentType = internalDoc.InternalDocumentTypeId };
        }

        return null;
    }

    private async Task<ConnectedDocument> CheckIncomingDocumentsByRegistrationNumber(int registrationNumber, int year)
    {
        var incomingDoc = await _dbContext.IncomingDocuments
            .FirstOrDefaultAsync(doc => doc.RegistrationNumber == registrationNumber && doc.RegistrationDate.Year == year);

        if (incomingDoc != null)
        {
            return new ConnectedDocument() { RegistrationNumber = incomingDoc.RegistrationNumber, Id = incomingDoc.Id, DocumentType = incomingDoc.DocumentTypeId };
        }

        return null;
    }

    public async Task<IncomingDocument> GetIncomingDocumentById(int id, CancellationToken cancellationToken)
    {
        var incomingDoc = await _dbContext.IncomingDocuments
            .FirstOrDefaultAsync(doc => doc.Id == id);
        return incomingDoc;
    }

    public async Task<OutgoingDocument> GetOutgoingDocumentById(int id, CancellationToken cancellationToken)
    {
        var outgoingDoc = await _dbContext.OutgoingDocuments
            .FirstOrDefaultAsync(doc => doc.Id == id);
        return outgoingDoc;
    }
}