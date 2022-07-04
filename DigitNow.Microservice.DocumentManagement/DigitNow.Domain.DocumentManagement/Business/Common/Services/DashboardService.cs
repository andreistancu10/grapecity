using AutoMapper;
using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Models;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IDashboardService
    {
        Task<long> CountAllDocumentsAsync(IList<Expression<Func<Document, bool>>> predicate, CancellationToken cancellationToken);
        Task<List<DocumentViewModel>> GetAllDocumentsAsync(IList<Expression<Func<Document, bool>>> predicates, int page, int count, CancellationToken cancellationToken);
    }

    public class DashboardService : IDashboardService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IDocumentService _documentService;
        private readonly IIdentityService _identityService;
        private readonly IIdentityAdapterClient _identityAdapterClient;

        public DashboardService(DocumentManagementDbContext dbContext,
            IMapper mapper,
            IDocumentService documentService,
            IIdentityService identityService,
            IIdentityAdapterClient identityAdapterClient)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _documentService = documentService;
            _identityService = identityService;
            _identityAdapterClient = identityAdapterClient;
        }

        public async Task<long> CountAllDocumentsAsync(IList<Expression<Func<Document, bool>>> predicates, CancellationToken cancellationToken)
        {
            var user = await GetCurrentUserAsync(cancellationToken);

            var result = default(long);
            if (user.Roles.ToList().Contains((long)UserRole.Mayor))
            {
                result = await _dbContext.Documents
                    .WhereAll(predicates)
                    .CountAsync(cancellationToken);
            }
            else
            {
                var relatedUserIds = await GetRelatedUserIdsASync(user, cancellationToken);

                result = await _dbContext.Documents
                    .WhereAll(predicates)
                    .Where(x => relatedUserIds.Contains(x.CreatedBy))
                    .CountAsync(cancellationToken);
            }

            return result;
        }

        public async Task<List<DocumentViewModel>> GetAllDocumentsAsync(IList<Expression<Func<Document, bool>>> predicates, int page, int count, CancellationToken cancellationToken)
        {
            var user = await GetCurrentUserAsync(cancellationToken);

            var documents = default(List<Document>);

            if (user.Roles.Contains((long)UserRole.Mayor))
            {
                documents = await _dbContext.Documents
                    .WhereAll(predicates)
                    .OrderByDescending(x => x.RegistrationDate)
                    .Skip((page - 1) * count)
                    .Take(count)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                var relatedUserIds = await GetRelatedUserIdsASync(user, cancellationToken);

                documents = await _dbContext.Documents
                    .WhereAll(predicates)
                    .Where(x => relatedUserIds.Contains(x.CreatedBy))
                    .OrderByDescending(x => x.RegistrationDate)
                    .Skip((page - 1) * count)
                    .Take(count)
                    .ToListAsync(cancellationToken);
            }

            var documentsRelationsBag = await GetDocumentsRelationsBagAsync(documents, cancellationToken);

            return await MapDocumentsAsync(documents, documentsRelationsBag, cancellationToken);
        }

        private async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken)
        {
            var userId = _identityService.GetCurrentUserId();
            var user = await _identityAdapterClient.GetUserByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new InvalidOperationException(); //TODO: Add not found exception

            return user;
        }

        private async Task<List<DocumentViewModel>> MapDocumentsAsync(IEnumerable<Document> documents, DocumentRelationsBag documentRelationsBag, CancellationToken cancellationToken)
        {
            var result = new List<DocumentViewModel>();

            var incomingDocuments = documents
                .Where(x => x.DocumentType == DocumentType.Incoming)
                .ToList();
            result.AddRange(await MapChildDocumentAsync<IncomingDocument>(incomingDocuments, documentRelationsBag, cancellationToken, x => x.WorkflowHistory));

            var internalDocuments = documents
                .Where(x => x.DocumentType == DocumentType.Internal)
                .ToList();
            result.AddRange(await MapChildDocumentAsync<InternalDocument>(internalDocuments, documentRelationsBag, cancellationToken));

            var outogingDocuments = documents
                .Where(x => x.DocumentType == DocumentType.Outgoing)
                .ToList();
            result.AddRange(await MapChildDocumentAsync<OutgoingDocument>(outogingDocuments, documentRelationsBag, cancellationToken, x => x.WorkflowHistory));

            return result;
        }

        private async Task<List<DocumentViewModel>> MapChildDocumentAsync<T>(List<Document> documents, DocumentRelationsBag documentRelationsBag, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes)
            where T : VirtualDocument
        {
            var documentIds = documents.Select(x => x.Id).ToList();

            var virtualDocuments = await _dbContext.Set<T>().AsQueryable()
                .Includes(includes)
                .Where(x => documentIds.Contains(x.DocumentId))
                .ToListAsync(cancellationToken);

            var documentRegistry = documents.ToDictionary(x => x, y => virtualDocuments.Where(x => x.DocumentId == y.Id));

            var result = new List<DocumentViewModel>();
            foreach (var registryEntry in documentRegistry)
            {
                var document = registryEntry.Key;

                foreach (var virtualDocument in registryEntry.Value)
                {
                    var viewModel = _mapper.Map<T, DocumentViewModel>(virtualDocument);
                    //SetUserRelationship(document, virtualDocument, viewModel);                    
                    //SetCategoryRelationship(document, virtualDocument, viewModel);
                    //SetInternalCategoryRelationship(document, virtualDocument, viewModel);
                    result.Add(viewModel);
                }
            }
            return result;
        }

        private async Task<IList<User>> GetRelatedUsersAsync(User user, CancellationToken cancellationToken)
        {
            if (user.Roles.ToList().Contains((long)UserRole.HeadOfDepartment))
            {
                var departmentId = user.Departments.FirstOrDefault();
                var departmentUsers = await _identityAdapterClient.GetUsersByDepartmentIdAsync(departmentId, cancellationToken);

                return departmentUsers.Users
                    .ToList();
            }

            return new List<User> { user };
        }

        private async Task<IEnumerable<long>> GetRelatedUserIdsASync(User user, CancellationToken cancellationToken) =>
            (await GetRelatedUsersAsync(user, cancellationToken)).Select(x => x.Id);

        private async Task<DocumentRelationsBag> GetDocumentsRelationsBagAsync(IList<Document> documents, CancellationToken cancellationToken)            
        {
            // TODO: Ask if we should show last user createdBy or modifiedBy
            var users = await GetRelatedUserRegistryAsync(cancellationToken);
            var documentCategories = await GetDocumentCategoriesAsync(cancellationToken);
            var internalDocumentCategories = await GetInternalDocumentCategoriesAsync(cancellationToken);

            return new DocumentRelationsBag
            {
                Users = users,
                Categories = documentCategories,
                InternalCategories = internalDocumentCategories
            };
        }

        private Task<Dictionary<long, User>> GetRelatedUserRegistryAsync(CancellationToken cancellationToken)
        {
            var registry = new Dictionary<long, User>
            {
                [1] = new User { Id = 1, Email = "test@test.com", Roles = new List<long> { 1 } },
                [2] = new User { Id = 2, Email = "test2@test.com", Roles = new List<long> { 2 } }
            };

            return Task.FromResult<Dictionary<long, User>>(registry);
        }

        private Task<Dictionary<long, object>> GetDocumentCategoriesAsync(CancellationToken cancellationToken)
        {
            var categories = new Dictionary<long, object>();

            return Task.FromResult(categories);
        }

        private Task<Dictionary<long, object>> GetInternalDocumentCategoriesAsync(CancellationToken cancellationToken)
        {
            var internalCategories = new Dictionary<long, object>();

            return Task.FromResult(internalCategories);
        }


        private class DocumentRelationsBag
        {
            public Dictionary<long, User> Users { get; set; }
            public Dictionary<long, object> Categories { get; set; }
            public Dictionary<long, object> InternalCategories { get; set; }
        }
    }
}
