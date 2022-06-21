using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Documents;
using DigitNow.Domain.DocumentManagement.Domain.Business.Common.Factories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services
{
    public interface IInternalDocumentService
    {
        Task<IList<InternalDocument>> FindAsync(Expression<Func<InternalDocument, bool>> query, CancellationToken cancellationToken);
        Task SetResolutionAsync(IList<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken);
    }

    public class InternalDocumentService : IInternalDocumentService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IDocumentService _documentService;

        public InternalDocumentService(DocumentManagementDbContext dbContext, IDocumentService documentService)
        {
            _dbContext = dbContext;
            _documentService = documentService;
        }

        public async Task<IList<InternalDocument>> FindAsync(Expression<Func<InternalDocument, bool>> query, CancellationToken cancellationToken)
        {
            return await _dbContext.InternalDocuments
                .Where(query)
                .ToListAsync(cancellationToken);
        }

        public async Task SetResolutionAsync(IList<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken)
        {
            var dbInternalDocuments = _dbContext.InternalDocuments.Where(x => documentIds.Contains(x.Id));

            foreach (var dbInternalDocument in dbInternalDocuments)
            {
                await _dbContext.DocumentResolutions
                    .AddAsync(DocumentResolutionFactory.Create(dbInternalDocument, resolutionType, remarks));
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
