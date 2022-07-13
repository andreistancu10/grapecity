using AutoMapper;
using DigitNow.Adapters.MS.Identity;
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IDashboardService
    {
        Task<long> CountAllDocumentsAsync(DocumentPreprocessFilter preprocessFilter, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken);

        Task<List<VirtualDocument>> GetAllDocumentsAsync(DocumentPreprocessFilter preprocessFilter, DocumentPostprocessFilter postprocessFilter, int page, int count, CancellationToken cancellationToken);
    }

    public class DashboardService : IDashboardService
    {
        #region [ Fields ]

        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IIdentityAdapterClient _identityAdapterClient;
        private readonly IAuthenticationClient _authenticationClient;

        #endregion

        #region [ Properties ]

        private int PreviousYear => DateTime.UtcNow.Year - 1;

        #endregion

        #region [ Construction ]

        public DashboardService(DocumentManagementDbContext dbContext,
            IMapper mapper,
            IIdentityService identityService,
            IIdentityAdapterClient identityAdapterClient,
            IAuthenticationClient identityManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _identityService = identityService;
            _identityAdapterClient = identityAdapterClient;
            _authenticationClient = identityManager;
        }

        #endregion

        #region [ IDashboardService ]

        public async Task<long> CountAllDocumentsAsync(DocumentPreprocessFilter preprocessFilter, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken)
        {
            var documentsQuery = await BuildPreprocessDocumentsQueryAsync(preprocessFilter, cancellationToken);
            if (postprocessFilter.IsEmpty())
                return await documentsQuery.CountAsync(cancellationToken);

            var lightweightDocuments = await documentsQuery
                .Select(x => new Document { Id = x.Id, DocumentType = x.DocumentType })
                .ToListAsync(cancellationToken);

            return await CountVirtualDocuments(lightweightDocuments, postprocessFilter, cancellationToken);
        }

        public async Task<List<VirtualDocument>> GetAllDocumentsAsync(DocumentPreprocessFilter preprocessFilter, DocumentPostprocessFilter postprocessFilter, int page, int count, CancellationToken cancellationToken)
        {
            var documentsQuery = await BuildPreprocessDocumentsQueryAsync(preprocessFilter, cancellationToken);

            var documents = await documentsQuery.OrderByDescending(x => x.CreatedAt)
                 .Skip((page - 1) * count)
                 .Take(count)
                 .Select(x => new Document { Id = x.Id, DocumentType = x.DocumentType })
                 .ToListAsync(cancellationToken);

            var virtualDocuments = await FetchVirtualDocuments(documents, postprocessFilter, cancellationToken);

            return virtualDocuments
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }

        #endregion

        #region [ IDashboardService - Internal - Count ]

        private async Task<long> CountVirtualDocuments(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken)
        {
            var result = default(long);

            result += await CountIncomingDocumentsAsync(documents, postprocessFilter, cancellationToken);
            result += await CountInternalDocumentsAsync(documents, postprocessFilter, cancellationToken);
            result += await CountOutgoingDocumentsAsync(documents, postprocessFilter, cancellationToken);

            return result;
        }

        private async Task<long> CountIncomingDocumentsAsync(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken)
        {
            var incomingDocumentsIds = documents
                    .Where(x => x.DocumentType == DocumentType.Incoming)
                    .Select(x => x.Id)
                    .ToList();
            if (!incomingDocumentsIds.Any())
                return default(long);

            var incomingDocumentsIncludes = PredicateFactory.CreateIncludesList<IncomingDocument>(x => x.WorkflowHistory);
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

            //TODO: Add workflow history once is implemented
            return await CountChildDocumentsAsync<InternalDocument>(internalDocumentsIds, postprocessFilter, null, cancellationToken);
        }

        private async Task<long> CountOutgoingDocumentsAsync(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken)
        {
            var outgoingDocumentsIds = documents
                .Where(x => x.DocumentType == DocumentType.Outgoing)
                .Select(x => x.Id)
                .ToList();
            if (!outgoingDocumentsIds.Any())
                return default(long);

            var outgoingDocumentsIncludes = PredicateFactory.CreateIncludesList<OutgoingDocument>(x => x.WorkflowHistory);
            return await CountChildDocumentsAsync(outgoingDocumentsIds, postprocessFilter, outgoingDocumentsIncludes, cancellationToken);
        }

        private async Task<long> CountChildDocumentsAsync<T>(IList<long> childDocumentIds, DocumentPostprocessFilter postprocessFilter, IList<Expression<Func<T, object>>> includes, CancellationToken cancellationToken)
            where T : VirtualDocument
        {
            return await BuildChildDocumentsFetchQuery<T>(childDocumentIds, postprocessFilter, includes)
                .CountAsync(cancellationToken);
        }

        #endregion

        #region [ IDashboardService - Internal - Get ]

        private async Task<List<VirtualDocument>> FetchVirtualDocuments(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken)
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

        private async Task<IList<IncomingDocument>> FetchIncomingDocumentsAsync(IList<Document> documents, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken)
        {
            var incomingDocumentsIds = documents
                    .Where(x => x.DocumentType == DocumentType.Incoming)
                    .Select(x => x.Id)
                    .ToList();
            if (!incomingDocumentsIds.Any())
                return new List<IncomingDocument>();

            var incomingDocumentsIncludes = PredicateFactory.CreateIncludesList<IncomingDocument>(x => x.Document, x => x.WorkflowHistory);
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

            //TODO: Add workflow history once is implemented
            var internalDocumentsIncludes = PredicateFactory.CreateIncludesList<InternalDocument>(x => x.Document);
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

            var outgoingDocumentsIncludes = PredicateFactory.CreateIncludesList<OutgoingDocument>(x => x.Document, x => x.WorkflowHistory);
            return await FetchChildDocumentsAsync(outgoingDocumentsIds, postprocessFilter, outgoingDocumentsIncludes, cancellationToken);
        }

        private async Task<IList<T>> FetchChildDocumentsAsync<T>(IList<long> childDocumentIds, DocumentPostprocessFilter postprocessFilter, IList<Expression<Func<T, object>>> includes, CancellationToken cancellationToken)
            where T : VirtualDocument
        {

            return await BuildChildDocumentsFetchQuery(childDocumentIds, postprocessFilter, includes)
                .ToListAsync(cancellationToken);
        }

        #endregion

        #region [ Relationships ]

        private async Task<IEnumerable<long>> GetRelatedUserIdsAsync(UserModel userModel, CancellationToken cancellationToken) =>
            (await GetRelatedUsersAsync(userModel, cancellationToken)).Select(x => x.Id);

        private async Task<IList<UserModel>> GetRelatedUsersAsync(UserModel userModel, CancellationToken cancellationToken)
        {
            if (IsRole(userModel, UserRole.HeadOfDepartment))
            {
                var usersResponse = await _identityAdapterClient.GetUsersAsync(cancellationToken);

                return usersResponse.Users
                    .Select(x => _mapper.Map<UserModel>(x))
                    .Append(userModel)
                    .ToList();
            }

            return new List<UserModel> { userModel };
        }

        private async Task<UserModel> GetCurrentUserAsync(CancellationToken cancellationToken)
        {
            var userId = _identityService.GetCurrentUserId();

            var getUserByIdResponse = await _authenticationClient.GetUserById(userId, cancellationToken);
            if (getUserByIdResponse == null)
                throw new InvalidOperationException(); //TODO: Add not found exception

            return _mapper.Map<UserModel>(getUserByIdResponse);
        }

        #endregion

        #region [ Utils - Preprocessing Filters ]

        private IList<Expression<Func<Document, bool>>> GetDocumentsPreprocessBuiltinPredicates() =>
            PredicateFactory.CreatePredicatesList<Document>(x => x.CreatedAt.Year >= PreviousYear);

        private IList<Expression<Func<Document, bool>>> GetDocumentsPreprocessPredicates(DocumentPreprocessFilter preprocessFilter) =>
            ExpressionFilterBuilderRegistry.GetDocumentPreprocessFilterBuilder(preprocessFilter).Build();

        private IList<Expression<Func<Document, bool>>> GetPreprocessPredicates(DocumentPreprocessFilter preprocessFilter)
        {
            var preprocessPredicates = GetDocumentsPreprocessBuiltinPredicates();
            if (!preprocessFilter.IsEmpty())
            {
                GetDocumentsPreprocessPredicates(preprocessFilter).ForEach(predicate => preprocessPredicates.Add(predicate));
            }
            return preprocessPredicates;
        }

        private async Task<IQueryable<Document>> BuildPreprocessDocumentsQueryAsync(DocumentPreprocessFilter documentFilter, CancellationToken cancellationToken)
        {
            var documentsQuery = _dbContext.Documents
                .WhereAll(GetPreprocessPredicates(documentFilter));

            var userModel = await GetCurrentUserAsync(cancellationToken);

            if (!IsRole(userModel, UserRole.Mayor))
            {
                var relatedUserIds = await GetRelatedUserIdsAsync(userModel, cancellationToken);

                documentsQuery = documentsQuery
                    .Where(x => relatedUserIds.Contains(x.CreatedBy));
            }

            return documentsQuery;
        }

        #endregion

        #region [ Utils - Postprocessing Filters ]

        private IList<Expression<Func<T, bool>>> GetPostprocessPredicates<T>(DocumentPostprocessFilter postprocessFilter)
                    where T : VirtualDocument
        {
            if (!postprocessFilter.IsEmpty())
            {
                return ExpressionFilterBuilderRegistry.GetDocumentPostprocessFilterBuilder<T>(postprocessFilter).Build();
            }
            return new List<Expression<Func<T, bool>>>();
        }

        #endregion

        #region [ Utils ]

        private bool IsRole(UserModel userModel, UserRole role) =>
            userModel.Roles.Contains((long)role);

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
                .WhereAll(GetPostprocessPredicates<T>(postprocessFilter))
                .Where(x => childDocumentIds.Contains(x.DocumentId));
        }

        #endregion
    }
}
