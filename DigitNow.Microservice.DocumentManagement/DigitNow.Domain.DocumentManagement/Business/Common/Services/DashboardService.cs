using AutoMapper;
using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IDashboardService
    {
        Task<long> CountAllDocumentsAsync(IList<Expression<Func<Document, bool>>> predicates, CancellationToken cancellationToken);
        Task<List<DocumentViewModel>> GetAllDocumentsAsync(IList<Expression<Func<Document, bool>>> predicates, int page, int count, CancellationToken cancellationToken);
    }

    public class DashboardService : IDashboardService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IIdentityAdapterClient _identityAdapterClient;
        private readonly ICatalogClient _catalogClient;
        private readonly IAuthenticationClient _authenticationClient;

        public DashboardService(DocumentManagementDbContext dbContext,
            IMapper mapper,
            IIdentityService identityService,
            IIdentityAdapterClient identityAdapterClient,
            ICatalogClient catalogClient,
            IAuthenticationClient identityManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _identityService = identityService;
            _identityAdapterClient = identityAdapterClient;
            _catalogClient = catalogClient;
            _authenticationClient = identityManager;
        }

        public async Task<long> CountAllDocumentsAsync(IList<Expression<Func<Document, bool>>> predicates, CancellationToken cancellationToken)
        {
            var userModel = await GetCurrentUserAsync(cancellationToken);

            var documentsCountQuery = default(IQueryable<Document>);

            if (userModel.Roles.ToList().Contains((long)UserRole.Mayor))
            {
                documentsCountQuery = _dbContext.Documents
                    .WhereAll(predicates);
            }
            else
            {
                var relatedUserIds = await GetRelatedUserIdsAsync(userModel, cancellationToken);

                documentsCountQuery = _dbContext.Documents
                    .WhereAll(predicates)
                    .Where(x => relatedUserIds.Contains(x.CreatedBy));
            }

            var result = await documentsCountQuery.CountAsync(cancellationToken);

            return result;
        }

        public async Task<List<DocumentViewModel>> GetAllDocumentsAsync(IList<Expression<Func<Document, bool>>> predicates, int page, int count, CancellationToken cancellationToken)
        {
            var userModel = await GetCurrentUserAsync(cancellationToken);

            var documentsQuery = default(IQueryable<Document>);

            if (userModel.Roles.Contains((long)UserRole.Mayor))
            {
                documentsQuery = _dbContext.Documents
                    .WhereAll(predicates)
                    .OrderByDescending(x => x.RegistrationDate)
                    .Skip((page - 1) * count)
                    .Take(count);
            }
            else
            {
                var relatedUserIds = await GetRelatedUserIdsAsync(userModel, cancellationToken);

                documentsQuery = _dbContext.Documents
                    .WhereAll(predicates)
                    .Where(x => relatedUserIds.Contains(x.CreatedBy))
                    .OrderByDescending(x => x.RegistrationDate)
                    .Skip((page - 1) * count)
                    .Take(count);
            }

            var documents = await documentsQuery.ToListAsync(cancellationToken);

            var documentsRelationsBag = await GetDocumentsRelationsBagAsync(documents, cancellationToken);

            return await MapDocumentsAsync(documents, documentsRelationsBag, cancellationToken);
        }

        private async Task<UserModel> GetCurrentUserAsync(CancellationToken cancellationToken)
        {
            var userId = _identityService.GetCurrentUserId();

            var getUserByIdResponse = await _authenticationClient.GetUserById(userId, cancellationToken);
            if (getUserByIdResponse == null)
                throw new UnauthorizedAccessException("Cannot retrieve request user!");                

            return _mapper.Map<UserModel>(getUserByIdResponse);            
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
                    // Note: ViewModels relationships can be loaded dynamically in future versions
                    var aggregate = new VirtualDocumentAggregate<T>
                    {
                        VirtualDocument = virtualDocument,
                        Users = documentRelationsBag.Users,
                        Categories = documentRelationsBag.Categories,
                        InternalCategories = documentRelationsBag.InternalCategories
                    };

                    result.Add(_mapper.Map<VirtualDocumentAggregate<T>, DocumentViewModel>(aggregate));
                }
            }
            return result;
        }

        private async Task<IList<UserModel>> GetRelatedUsersAsync(UserModel userModel, CancellationToken cancellationToken)
        {
            if (userModel.Roles.ToList().Contains((long)UserRole.HeadOfDepartment))
            {
                var usersResponse = await _identityAdapterClient.GetUsersAsync(cancellationToken);
                
                return usersResponse.Users
                    .Select(x => _mapper.Map<UserModel>(x))
                    .Append(userModel)
                    .ToList();                
            }

            return new List<UserModel> { userModel };
        }

        private async Task<IEnumerable<long>> GetRelatedUserIdsAsync(UserModel userModel, CancellationToken cancellationToken) =>
            (await GetRelatedUsersAsync(userModel, cancellationToken)).Select(x => x.Id);

        private async Task<DocumentRelationsBag> GetDocumentsRelationsBagAsync(IList<Document> documents, CancellationToken cancellationToken)
        {
            var users = await GetRelatedUserRegistryAsync(documents, cancellationToken);
            var documentCategories = await GetDocumentCategoriesAsync(cancellationToken);
            var internalDocumentCategories = await GetInternalDocumentCategoriesAsync(cancellationToken);

            return new DocumentRelationsBag
            {
                Users = users,
                Categories = documentCategories,
                InternalCategories = internalDocumentCategories
            };
        }

        private async Task<List<User>> GetRelatedUserRegistryAsync(IList<Document> documents, CancellationToken cancellationToken)
        {
            var createdByUsers = documents
                .Select(x => x.CreatedBy)
                .ToList();

            var usersList = await _authenticationClient.GetUsersWithExtensions(cancellationToken);

            var relatedUsers = usersList.UserExtensions
                .Where(x => createdByUsers.Contains(x.Id))
                .Select(x => new User
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Active = x.Active,
                    Email = x.UserName
                })
                .ToList();

            return relatedUsers;
        }

        private async Task<List<DocumentCategoryModel>> GetDocumentCategoriesAsync(CancellationToken cancellationToken)
        {
            var documentTypesResponse = await _catalogClient.DocumentTypes.GetDocumentTypesAsync(cancellationToken);

            // Note: DocumentTypes is actual DocumentCategory
            var documentCategoryModels = documentTypesResponse.DocumentTypes
                .Select(x => _mapper.Map<DocumentCategoryModel>(x))
                .ToList();

            return documentCategoryModels;
        }

        private async Task<List<InternalDocumentCategoryModel>> GetInternalDocumentCategoriesAsync(CancellationToken cancellationToken)
        {
            var internalDocumentTypesResponse = await _catalogClient.InternalDocumentTypes.GetInternalDocumentTypesAsync(cancellationToken);

            // Note: DocumentTypes is actual DocumentCategory
            var internalDocumentCategoryModels = internalDocumentTypesResponse.InternalDocumentTypes
                .Select(x => _mapper.Map<InternalDocumentCategoryModel>(x))
                .ToList();

            return internalDocumentCategoryModels;
        }


        private class DocumentRelationsBag
        {
            public IReadOnlyList<User> Users { get; set; }
            public IReadOnlyList<DocumentCategoryModel> Categories { get; set; }
            public IReadOnlyList<InternalDocumentCategoryModel> InternalCategories { get; set; }
        }
    }
}
