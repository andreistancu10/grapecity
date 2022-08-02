using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IVirtualDocumentService
    {
        Task<List<VirtualDocument>> FindAllAsync<T>(
            Expression<Func<VirtualDocument, bool>> predicate,
            CancellationToken cancellationToken,
            params Expression<Func<T, object>>[] includes) where T : VirtualDocument;

        Task<long> CountVirtualDocuments(IList<Document> documents, CancellationToken cancellationToken);
        Task<long> CountVirtualDocuments(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken);

        Task<List<VirtualDocument>> FetchVirtualDocuments(IList<Document> documents, CancellationToken cancellationToken);
        Task<List<VirtualDocument>> FetchVirtualDocuments(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken);
    }

    public class VirtualDocumentService : IVirtualDocumentService
    {
        private readonly DocumentManagementDbContext _dbContext;

        public VirtualDocumentService(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<VirtualDocument>> FindAllAsync<T>(
            Expression<Func<VirtualDocument, bool>> predicate,
            CancellationToken cancellationToken,
            params Expression<Func<T, object>>[] includes) where T : VirtualDocument
        {
            return _dbContext.Set<T>()
                .Includes(includes)
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public Task<long> CountVirtualDocuments(IList<Document> documents, CancellationToken cancellationToken) => 
            CountVirtualDocuments(documents, DocumentPostprocessFilter.Empty, cancellationToken);

        public async Task<long> CountVirtualDocuments(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken)
        {
            var result = default(long);

            result += await CountIncomingDocumentsAsync(documents, postprocessFilter, cancellationToken);
            result += await CountInternalDocumentsAsync(documents, postprocessFilter, cancellationToken);
            result += await CountOutgoingDocumentsAsync(documents, postprocessFilter, cancellationToken);

            return result;
        }

        public Task<List<VirtualDocument>> FetchVirtualDocuments(IList<Document> documents, CancellationToken cancellationToken) =>
            FetchVirtualDocuments(documents, DocumentPostprocessFilter.Empty, cancellationToken);

        public async Task<List<VirtualDocument>> FetchVirtualDocuments(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken)
        {
            var result = new List<VirtualDocument>();

            var incomingDocuments = await FetchIncomingDocumentsAsync(documents, postprocessFilter, cancellationToken);
            if (incomingDocuments.Any())
            {
                result.AddRange(incomingDocuments);
            }

            var internalDocuments = await FetchInternalDocumentsAsync(documents, postprocessFilter, cancellationToken);
            if (internalDocuments.Any())
            {
                result.AddRange(internalDocuments);
            }

            var outgoingDocuments = await FetchOutgoingDocumentsAsync(documents, postprocessFilter, cancellationToken);
            if (outgoingDocuments.Any())
            {
                result.AddRange(outgoingDocuments);
            }

            return result;
        }

        #region [ IDashboardService - Internal - Count ]

        private async Task<long> CountIncomingDocumentsAsync(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken)
        {
            var incomingDocumentsIds = documents
                .Where(x => x.DocumentType == DocumentType.Incoming)
                .Select(x => x.Id)
                .ToList();
            if (!incomingDocumentsIds.Any())
                return default(long);

            var incomingDocumentsIncludes = PredicateFactory.CreateIncludesList<IncomingDocument>(x => x.Document.WorkflowHistories);
            return await CountChildDocumentsAsync(incomingDocumentsIds, postprocessFilter, incomingDocumentsIncludes, cancellationToken);
        }

        private async Task<long> CountInternalDocumentsAsync(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken)
        {
            var internalDocumentsIds = documents
                .Where(x => x.DocumentType == DocumentType.Internal)
                .Select(x => x.Id)
                .ToList();
            if (!internalDocumentsIds.Any())
                return default(long);

            var internalDocumentsIncludes = PredicateFactory.CreateIncludesList<InternalDocument>(x => x.Document.WorkflowHistories);
            return await CountChildDocumentsAsync<InternalDocument>(internalDocumentsIds, postprocessFilter, internalDocumentsIncludes, cancellationToken);
        }

        private async Task<long> CountOutgoingDocumentsAsync(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken)
        {
            var outgoingDocumentsIds = documents
                .Where(x => x.DocumentType == DocumentType.Outgoing)
                .Select(x => x.Id)
                .ToList();
            if (!outgoingDocumentsIds.Any())
                return default(long);

            var outgoingDocumentsIncludes = PredicateFactory.CreateIncludesList<OutgoingDocument>(x => x.Document.WorkflowHistories);
            return await CountChildDocumentsAsync(outgoingDocumentsIds, postprocessFilter, outgoingDocumentsIncludes, cancellationToken);
        }

        private async Task<long> CountChildDocumentsAsync<T>(IList<long> childDocumentIds, DocumentPostprocessFilter postprocessFilter, IList<Expression<Func<T, object>>> includes, CancellationToken cancellationToken)
            where T : VirtualDocument
        {
            return await BuildChildDocumentsFetchQuery<T>(childDocumentIds, postprocessFilter, includes)
                .CountAsync(cancellationToken);
        }

        #endregion

        #region [ VirtualDocument - Internal Fetch ]

        private async Task<IList<IncomingDocument>> FetchIncomingDocumentsAsync(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken)
        {
            var incomingDocumentsIds = documents
                .Where(x => x.DocumentType == DocumentType.Incoming)
                .Select(x => x.Id)
                .ToList();
            if (!incomingDocumentsIds.Any())
                return new List<IncomingDocument>();

            var incomingDocumentsIncludes = PredicateFactory.CreateIncludesList<IncomingDocument>(x => x.Document, x => x.Document.WorkflowHistories);
            return await FetchChildDocumentsAsync(incomingDocumentsIds, postprocessFilter, incomingDocumentsIncludes, cancellationToken);
        }

        private async Task<IList<InternalDocument>> FetchInternalDocumentsAsync(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken)
        {
            var internalDocumentsIds = documents
                .Where(x => x.DocumentType == DocumentType.Internal)
                .Select(x => x.Id)
                .ToList();
            if (!internalDocumentsIds.Any())
                return new List<InternalDocument>();

            var internalDocumentsIncludes = PredicateFactory.CreateIncludesList<InternalDocument>(x => x.Document, x => x.Document.WorkflowHistories);
            return await FetchChildDocumentsAsync(internalDocumentsIds, postprocessFilter, internalDocumentsIncludes, cancellationToken);
        }

        private async Task<IList<OutgoingDocument>> FetchOutgoingDocumentsAsync(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken)
        {
            var outgoingDocumentsIds = documents
                .Where(x => x.DocumentType == DocumentType.Outgoing)
                .Select(x => x.Id)
                .ToList();
            if (!outgoingDocumentsIds.Any())
                return new List<OutgoingDocument>();

            var outgoingDocumentsIncludes = PredicateFactory.CreateIncludesList<OutgoingDocument>(x => x.Document, x => x.Document.WorkflowHistories);
            return await FetchChildDocumentsAsync(outgoingDocumentsIds, postprocessFilter, outgoingDocumentsIncludes, cancellationToken);
        }

        private async Task<IList<T>> FetchChildDocumentsAsync<T>(IList<long> childDocumentIds, DocumentPostprocessFilter postprocessFilter, IList<Expression<Func<T, object>>> includes, CancellationToken cancellationToken)
            where T : VirtualDocument
        {
            return await BuildChildDocumentsFetchQuery(childDocumentIds, postprocessFilter, includes)
                .ToListAsync(cancellationToken);
        }

        #endregion

        #region [ Helpers ]

        private IQueryable<T> BuildChildDocumentsFetchQuery<T>(IList<long> childDocumentIds, DocumentPostprocessFilter postprocessFilter, IList<Expression<Func<T, object>>> includes)
            where T : VirtualDocument
        {
            var virtualDocumentsQuery = _dbContext.Set<T>().AsQueryable();

            if (includes != null)
            {
                virtualDocumentsQuery = virtualDocumentsQuery.Includes(includes);
            }

            return virtualDocumentsQuery            
                .Include(x => x.Document)
                .WhereAll(GetPostprocessPredicates<T>(postprocessFilter))
                .Where(x => childDocumentIds.Contains(x.DocumentId));
        }

        #endregion

        #region [ Utils - Postprocessing Filters ]

        private IList<Expression<Func<T, bool>>> GetPostprocessPredicates<T>(DocumentPostprocessFilter postprocessFilter)
            where T : VirtualDocument
        {
            if (postprocessFilter == null || !postprocessFilter.IsEmpty())
            {
                return ExpressionFilterBuilderRegistry.GetDocumentPostprocessFilterBuilder<T>(_dbContext, postprocessFilter).Build();
            }
            return new List<Expression<Func<T, bool>>>();
        }

        #endregion
    }
}