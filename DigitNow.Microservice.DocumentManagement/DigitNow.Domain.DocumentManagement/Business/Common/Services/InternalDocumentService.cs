using DigitNow.Domain.DocumentManagement.Business.Common.Repositories;
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
    public interface IInternalDocumentService
    {
        Task<List<InternalDocument>> FindAsync(Expression<Func<InternalDocument, bool>> predicate, CancellationToken cancellationToken);
        Task SetResolutionAsync(IList<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken);
    }

    public class InternalDocumentService : IInternalDocumentService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IDocumentService _documentService;
        private readonly IInternalDocumentRepository _internalDocumentRepository;

        public InternalDocumentService(DocumentManagementDbContext dbContext, 
            IDocumentService documentService, 
            IInternalDocumentRepository internalDocumentRepository)
        {
            _dbContext = dbContext;
            _documentService = documentService;
            _internalDocumentRepository = internalDocumentRepository;
        }

        public Task<List<InternalDocument>> FindAsync(Expression<Func<InternalDocument, bool>> predicate, CancellationToken cancellationToken) =>
            _internalDocumentRepository.FindByAsync(predicate, cancellationToken);

        public async Task SetResolutionAsync(IList<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken)
        {
            var dbInternalDocuments = await _dbContext.InternalDocuments
                .Include(x => x.Document)
                .Where(x => documentIds.Contains(x.Id))
                .ToListAsync(cancellationToken);

            foreach (var dbInternalDocument in dbInternalDocuments)
            {
                await _dbContext.DocumentResolutions
                    .AddAsync(DocumentResolutionFactory.Create(dbInternalDocument, resolutionType, remarks));
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
