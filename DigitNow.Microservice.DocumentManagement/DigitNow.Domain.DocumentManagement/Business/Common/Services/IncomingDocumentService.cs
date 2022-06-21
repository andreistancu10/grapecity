using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Documents;
using DigitNow.Domain.DocumentManagement.Domain.Business.Common.Factories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services
{
    public interface IIncomingDocumentService
    {
        Task<IList<IncomingDocument>> FindAsync(Expression<Func<IncomingDocument, bool>> query, CancellationToken cancellationToken);
        Task SetResolutionAsync(IList<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken);
    }

    public class IncomingDocumentService : IIncomingDocumentService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IDocumentService _documentService;

        public IncomingDocumentService(DocumentManagementDbContext dbContext, IDocumentService documentService)
        {
            _dbContext = dbContext;
            _documentService = documentService;
        }

        public async Task<IList<IncomingDocument>> FindAsync(Expression<Func<IncomingDocument, bool>> query, CancellationToken cancellationToken)
        {
            return await _dbContext.IncomingDocuments
                .Where(query)
                .ToListAsync(cancellationToken);
        }

        public async Task SetResolutionAsync(IList<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken)
        {
            var dbIncomingDocuments = _dbContext.IncomingDocuments.Where(x => documentIds.Contains(x.Id));

            foreach (var dbIncomingDocument in dbIncomingDocuments)
            {
                await _dbContext.DocumentResolutions
                    .AddAsync(DocumentResolutionFactory.Create(dbIncomingDocument, resolutionType, remarks));
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
