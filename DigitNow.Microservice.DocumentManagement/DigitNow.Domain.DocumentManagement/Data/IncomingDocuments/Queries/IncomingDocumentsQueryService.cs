using DigitNow.Domain.DocumentManagement.Data.ConnectedDocuments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.Queries
{
    public interface IDocumentsQueryService
    {
        Task<ConnectedDocument> GetDocsByRegistrationNumberAndYear(int registrationNumber, int year, CancellationToken cancellationToken);
    }
    public class IncomingDocumentsQueryService : IDocumentsQueryService
    {
        protected readonly DocumentManagementDbContext _dbContext;

        public IncomingDocumentsQueryService(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ConnectedDocument> GetDocsByRegistrationNumberAndYear(int registrationNumber, int year, CancellationToken cancellationToken)
        {
            var incomingDoc = await CheckIncomingDocumentsByRegistrationNumber(registrationNumber, year);
            if (incomingDoc != null)
                return incomingDoc;

            //TODO Implement methods
            var internalDoc = await CheckInternalDocumentsByRegistrationNumber(registrationNumber, year);
            if (incomingDoc != null)
                return internalDoc;


            return await CheckOutgoingDocumentsByRegistrationNumber(registrationNumber, year);
        }

        private Task<ConnectedDocument> CheckInternalDocumentsByRegistrationNumber(int registrationNumber, int year)
        {
            throw new NotImplementedException();
        }

        private Task<ConnectedDocument> CheckOutgoingDocumentsByRegistrationNumber(int registrationNumber, int year)
        {
            throw new NotImplementedException();
        }

        private async Task<ConnectedDocument> CheckIncomingDocumentsByRegistrationNumber(int registrationNumber, int year)
        {
            var incomingDoc = await _dbContext.IncomingDocuments
                .FirstOrDefaultAsync(doc => doc.RegistrationNumber == registrationNumber && doc.CreationDate.Year == year);

            if (incomingDoc != null)
            {
                return new ConnectedDocument() { RegistrationNumber = incomingDoc.RegistrationNumber, Id = incomingDoc.Id, DocumentType = incomingDoc.DocumentTypeId };
            }

            return null;
        }
    }
}