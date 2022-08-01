using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Documents.Postprocess;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Documents.Postprocess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IVirtualDocumentService
    {
        Task<List<VirtualDocument>> FindAllAsync<T>(
            Expression<Func<VirtualDocument, bool>> predicate,
            CancellationToken token,
            params Expression<Func<T, object>>[] includes) where T : VirtualDocument;

        Task<long> CountVirtualDocuments(IList<Document> documents, CancellationToken token);
        Task<long> CountVirtualDocuments(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken token);

        Task<List<VirtualDocument>> FetchVirtualDocuments(IList<Document> documents, CancellationToken token);
        Task<List<VirtualDocument>> FetchVirtualDocuments(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken token);

        Task<IQueryable<T>> FetchVirtualDocuments<T>(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken token);
    }

    public class VirtualDocumentService : IVirtualDocumentService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;

        public VirtualDocumentService(
            DocumentManagementDbContext dbContext,
            IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
        }

        public Task<List<VirtualDocument>> FindAllAsync<T>(
            Expression<Func<VirtualDocument, bool>> predicate,
            CancellationToken token,
            params Expression<Func<T, object>>[] includes) where T : VirtualDocument
        {
            return _dbContext.Set<T>()
                .Includes(includes)
                .Where(predicate)
                .ToListAsync(token);
        }

        public Task<long> CountVirtualDocuments(IList<Document> documents, CancellationToken token) =>
            CountVirtualDocuments(documents, DocumentPostprocessFilter.Empty, token);

        public async Task<long> CountVirtualDocuments(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken token)
        {
            var result = default(long);

            result += await CountIncomingDocumentsAsync(documents, postprocessFilter, token);
            result += await CountInternalDocumentsAsync(documents, postprocessFilter, token);
            result += await CountOutgoingDocumentsAsync(documents, postprocessFilter, token);

            return result;
        }

        public Task<List<VirtualDocument>> FetchVirtualDocuments(IList<Document> documents, CancellationToken token) =>
            FetchVirtualDocuments(documents, DocumentPostprocessFilter.Empty, token);

        public async Task<List<VirtualDocument>> FetchVirtualDocuments(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken token)
        {
            var result = new List<VirtualDocument>();

            var incomingDocuments = await FetchIncomingDocumentsAsync(documents, postprocessFilter, token);
            if (incomingDocuments.Any())
            {
                result.AddRange(incomingDocuments);
            }

            var internalDocuments = await FetchInternalDocumentsAsync(documents, postprocessFilter, token);
            if (internalDocuments.Any())
            {
                result.AddRange(internalDocuments);
            }

            var outgoingDocuments = await FetchOutgoingDocumentsAsync(documents, postprocessFilter, token);
            if (outgoingDocuments.Any())
            {
                result.AddRange(outgoingDocuments);
            }

            return result;
        }

        #region [ IDashboardService - Internal - Count ]

        private async Task<long> CountIncomingDocumentsAsync(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken token)
        {
            var incomingDocumentsIds = documents
                .Where(x => x.DocumentType == DocumentType.Incoming)
                .Select(x => x.Id)
                .ToList();
            if (!incomingDocumentsIds.Any())
                return default(long);

            var incomingDocumentsIncludes = PredicateFactory.CreateIncludesList<IncomingDocument>(x => x.Document.WorkflowHistories);
            return await CountChildDocumentsAsync(incomingDocumentsIds, postprocessFilter, incomingDocumentsIncludes, token);
        }

        private async Task<long> CountInternalDocumentsAsync(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken token)
        {
            var internalDocumentsIds = documents
                .Where(x => x.DocumentType == DocumentType.Internal)
                .Select(x => x.Id)
                .ToList();
            if (!internalDocumentsIds.Any())
                return default(long);

            var internalDocumentsIncludes = PredicateFactory.CreateIncludesList<InternalDocument>(x => x.Document.WorkflowHistories);
            return await CountChildDocumentsAsync<InternalDocument>(internalDocumentsIds, postprocessFilter, internalDocumentsIncludes, token);
        }

        private async Task<long> CountOutgoingDocumentsAsync(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken token)
        {
            var outgoingDocumentsIds = documents
                .Where(x => x.DocumentType == DocumentType.Outgoing)
                .Select(x => x.Id)
                .ToList();
            if (!outgoingDocumentsIds.Any())
                return default(long);

            var outgoingDocumentsIncludes = PredicateFactory.CreateIncludesList<OutgoingDocument>(x => x.Document.WorkflowHistories);
            return await CountChildDocumentsAsync(outgoingDocumentsIds, postprocessFilter, outgoingDocumentsIncludes, token);
        }

        private async Task<long> CountChildDocumentsAsync<T>(IList<long> childDocumentIds, DocumentPostprocessFilter postprocessFilter, IList<Expression<Func<T, object>>> includes, CancellationToken token)
            where T : VirtualDocument
        {
            var childDocumentsQuery = await BuildChildDocumentsFetchQueryAsync<T>(childDocumentIds, postprocessFilter, includes, token);

            return await childDocumentsQuery.CountAsync(token);
        }

        #endregion

        #region [ VirtualDocument - Internal Fetch ]

        private async Task<IList<IncomingDocument>> FetchIncomingDocumentsAsync(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken token)
        {
            var incomingDocumentsIds = documents
                .Where(x => x.DocumentType == DocumentType.Incoming)
                .Select(x => x.Id)
                .ToList();
            if (!incomingDocumentsIds.Any())
                return new List<IncomingDocument>();

            var incomingDocumentsIncludes = PredicateFactory.CreateIncludesList<IncomingDocument>(x => x.Document, x => x.Document.WorkflowHistories);
            return await FetchChildDocumentsAsync(incomingDocumentsIds, postprocessFilter, incomingDocumentsIncludes, token);
        }

        private async Task<IList<InternalDocument>> FetchInternalDocumentsAsync(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken token)
        {
            var internalDocumentsIds = documents
                .Where(x => x.DocumentType == DocumentType.Internal)
                .Select(x => x.Id)
                .ToList();
            if (!internalDocumentsIds.Any())
                return new List<InternalDocument>();

            var internalDocumentsIncludes = PredicateFactory.CreateIncludesList<InternalDocument>(x => x.Document, x => x.Document.WorkflowHistories);
            return await FetchChildDocumentsAsync(internalDocumentsIds, postprocessFilter, internalDocumentsIncludes, token);
        }

        private async Task<IList<OutgoingDocument>> FetchOutgoingDocumentsAsync(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken token)
        {
            var outgoingDocumentsIds = documents
                .Where(x => x.DocumentType == DocumentType.Outgoing)
                .Select(x => x.Id)
                .ToList();
            if (!outgoingDocumentsIds.Any())
                return new List<OutgoingDocument>();

            var outgoingDocumentsIncludes = PredicateFactory.CreateIncludesList<OutgoingDocument>(x => x.Document, x => x.Document.WorkflowHistories);
            return await FetchChildDocumentsAsync(outgoingDocumentsIds, postprocessFilter, outgoingDocumentsIncludes, token);
        }

        private async Task<IList<T>> FetchChildDocumentsAsync<T>(IList<long> childDocumentIds, DocumentPostprocessFilter postprocessFilter, IList<Expression<Func<T, object>>> includes, CancellationToken token)
            where T : VirtualDocument
        {
            var childDocumentsQuery = await BuildChildDocumentsFetchQueryAsync(childDocumentIds, postprocessFilter, includes, token);

            return await childDocumentsQuery.ToListAsync(token);
        }

        #endregion

        #region [ Helpers ]

        private async Task<IQueryable<T>> BuildChildDocumentsFetchQueryAsync<T>(IList<long> childDocumentIds, DocumentPostprocessFilter postprocessFilter, IList<Expression<Func<T, object>>> includes, CancellationToken token)
            where T : VirtualDocument
        {
            var virtualDocumentsQuery = _dbContext.Set<T>().AsQueryable();

            if (includes != null)
            {
                virtualDocumentsQuery = virtualDocumentsQuery.Includes(includes);
            }

            return virtualDocumentsQuery
                .Include(x => x.Document)
                .WhereAll((await GetPostprocessPredicatesAsync<T>(postprocessFilter, token)).ToPredicates())
                .Where(x => childDocumentIds.Contains(x.DocumentId));
        }

        #endregion

        #region [ Utils - Postprocessing Filters ]

        private Task<DataExpressions<T>> GetPostprocessPredicatesAsync<T>(DocumentPostprocessFilter postprocessFilter, CancellationToken token)
            where T : VirtualDocument
        {
            //DevHint: We can add multiple postprocess filters in this method

            return GetActivePostprocessFilter<T>(postprocessFilter, token);
        }

        private Task<DataExpressions<T>> GetActivePostprocessFilter<T>(DocumentPostprocessFilter postprocessFilter, CancellationToken token)
            where T : VirtualDocument
        {
            var filterComponent = new ActiveDocumentsPostprocessFilterComponent<T>(_serviceProvider);
            var filterComponentContext = new ActiveDocumentsPostprocessFilterComponentContext
            {
                PostprocessFilter = postprocessFilter
            };

            return filterComponent.ExtractDataExpressionsAsync(filterComponentContext, token);
        }

        #endregion
    }
}