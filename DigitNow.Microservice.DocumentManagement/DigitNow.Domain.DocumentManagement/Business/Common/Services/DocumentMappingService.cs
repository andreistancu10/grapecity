using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteBuilders;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IDocumentMappingService
    {
        Task<List<DocumentViewModel>> MapToDocumentViewModelAsync(IList<Document> documents, CancellationToken cancellationToken);
        Task<List<DocumentViewModel>> MapToDocumentViewModelAsync(IList<Document> documents, DocumentFilter documentFilter, CancellationToken cancellationToken);

        Task<List<ReportViewModel>> MapToReportViewModelAsync(IList<Document> documents, CancellationToken cancellationToken);
        Task<List<ReportViewModel>> MapToReportViewModelAsync(IList<Document> documents, DocumentFilter documentFilter, CancellationToken cancellationToken);
    }

    public class DocumentMappingService : IDocumentMappingService
    {
        #region [ Fields ]

        private readonly DocumentManagementDbContext _dbContext;
        private readonly DocumentRelationsFetcher _documentRelationsFetcher;
        private readonly IMapper _mapper;
        
        #endregion

        #region [ Construction ]

        public DocumentMappingService(
            DocumentManagementDbContext dbContext,
            IServiceProvider serviceProvider,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _documentRelationsFetcher = new DocumentRelationsFetcher(serviceProvider);
        }

        #endregion

        #region [ IDocumentMappingService ]

        public async Task<List<DocumentViewModel>> MapToDocumentViewModelAsync(IList<Document> documents, CancellationToken cancellationToken)
        {
            await _documentRelationsFetcher.FetchRelationshipsAsync(new DocumentsFetcherContext { Documents = documents }, cancellationToken);
            return await MapDocumentsAsync<DocumentViewModel>(documents, null, cancellationToken);
        }

        public async Task<List<DocumentViewModel>> MapToDocumentViewModelAsync(IList<Document> documents, DocumentFilter documentFilter, CancellationToken cancellationToken)
        {
            await _documentRelationsFetcher.FetchRelationshipsAsync(new DocumentsFetcherContext { Documents = documents }, cancellationToken);
            return await MapDocumentsAsync<DocumentViewModel>(documents, documentFilter, cancellationToken);
        }


        public async Task<List<ReportViewModel>> MapToReportViewModelAsync(IList<Document> documents, DocumentFilter documentFilter, CancellationToken cancellationToken)
        {
            await _documentRelationsFetcher.FetchRelationshipsAsync(new DocumentsFetcherContext { Documents = documents }, cancellationToken);
            return await MapDocumentsAsync<ReportViewModel>(documents, documentFilter, cancellationToken);
        }

        public async Task<List<ReportViewModel>> MapToReportViewModelAsync(IList<Document> documents, CancellationToken cancellationToken)
        {
            await _documentRelationsFetcher.FetchRelationshipsAsync(new DocumentsFetcherContext { Documents = documents }, cancellationToken);
            return await MapDocumentsAsync<ReportViewModel>(documents, null, cancellationToken);
        }

        #endregion

        #region [ Internal ]

        private async Task<List<TViewModel>> MapDocumentsAsync<TViewModel>(IList<Document> documents, DocumentFilter documentFilter, CancellationToken cancellationToken)
            where TViewModel : class, new()
        {
            var result = new List<TViewModel>();

            var incomingDocuments = await FetchIncomingDocumentsAsync(documents, documentFilter, cancellationToken);
            if (incomingDocuments.Any())
            {
                result.AddRange(MapChildDocument<IncomingDocument, TViewModel>(incomingDocuments));
            }

            var internalDocuments = await FetchInternalDocumentsAsync(documents, documentFilter, cancellationToken);
            if (internalDocuments.Any())
            {
                result.AddRange(MapChildDocument<InternalDocument, TViewModel>(internalDocuments));
            }

            var outgoingDocuments = await FetchOutgoingDocumentsAsync(documents, documentFilter, cancellationToken);
            if (outgoingDocuments.Any())
            {
                result.AddRange(MapChildDocument<OutgoingDocument, TViewModel>(outgoingDocuments));
            }
            
            return result;            
        }

        private Task<IList<IncomingDocument>> FetchIncomingDocumentsAsync(IList<Document> documents, DocumentFilter documentFilter, CancellationToken cancellationToken)
        {
            var incomingDocumentsIds = documents
                    .Where(x => x.DocumentType == DocumentType.Incoming)
                    .Select(x => x.Id)
                    .ToList();
            var incomingDocumentsIncludes = PredicateFactory.CreateIncludesList<IncomingDocument>(x => x.WorkflowHistory);
            return FetchChildDocumentsAsync(incomingDocumentsIds, documentFilter, incomingDocumentsIncludes, cancellationToken);
        }

        private Task<IList<InternalDocument>> FetchInternalDocumentsAsync(IList<Document> documents, DocumentFilter documentFilter, CancellationToken cancellationToken)
        {
            var internalDocumentsIds = documents
                .Where(x => x.DocumentType == DocumentType.Internal)
                .Select(x => x.Id)
                .ToList();
            //TODO: Add workflow history once is implemented
            return FetchChildDocumentsAsync<InternalDocument>(internalDocumentsIds, documentFilter, null, cancellationToken);
        }

        private Task<IList<OutgoingDocument>> FetchOutgoingDocumentsAsync(IList<Document> documents, DocumentFilter documentFilter, CancellationToken cancellationToken)
        {
            var outgoingDocumentsIds = documents
                .Where(x => x.DocumentType == DocumentType.Outgoing)
                .Select(x => x.Id)
                .ToList();
            var outgoingDocumentsIncludes = PredicateFactory.CreateIncludesList<OutgoingDocument>(x => x.WorkflowHistory);
            return FetchChildDocumentsAsync(outgoingDocumentsIds, documentFilter, outgoingDocumentsIncludes, cancellationToken);
        }

        private List<TResult> MapChildDocument<T, TResult>(IList<T> childDocuments)
            where T : VirtualDocument
            where TResult : class, new()
        {
            var result = new List<TResult>();
            foreach (var childDocument in childDocuments)
            {
                var aggregate = new VirtualDocumentAggregate<T>
                {
                    VirtualDocument = childDocument,                    
                    Users = _documentRelationsFetcher.DocumentUsers,
                    Categories = _documentRelationsFetcher.DocumentCategories,
                    InternalCategories = _documentRelationsFetcher.DocumentInternalCategories
                };

                result.Add(_mapper.Map<VirtualDocumentAggregate<T>, TResult>(aggregate));
            }
            return result;            
        }

        private async Task<IList<T>> FetchChildDocumentsAsync<T>(IList<long> childDocumentIds, DocumentFilter documentFilter, IList<Expression<Func<T, object>>> includes, CancellationToken cancellationToken)
            where T: VirtualDocument
        {
            var virtualDocumentsQuery = _dbContext.Set<T>().AsQueryable();
            
            if (includes != null)
            {
                virtualDocumentsQuery.Includes(includes);
            }

            virtualDocumentsQuery.Where(x => childDocumentIds.Contains(x.DocumentId));

            if (documentFilter != null)
            {
                var categoriesIds = _documentRelationsFetcher.DocumentCategories
                    .Select(x => x.Id).ToList();

                var internalCategoriesIds = _documentRelationsFetcher.DocumentInternalCategories
                    .Select(x => x.Id).ToList();

                var categoryPredicates = new DocumentCategoryFilterBuilder<T>(documentFilter, categoriesIds, internalCategoriesIds)
                    .Build();

                virtualDocumentsQuery.WhereAll(categoryPredicates);
            }

            return await virtualDocumentsQuery.ToListAsync(cancellationToken);
        }

        #endregion
    }
}