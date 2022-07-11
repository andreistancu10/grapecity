using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisterMapping;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services;

public interface IDocumentMappingService
{
    Task<List<DocumentViewModel>> MapToDocumentViewModelAsync(IList<Document> documents, CancellationToken cancellationToken);
    Task<List<ReportViewModel>> MapToReportViewModelAsync(IList<Document> documents, CancellationToken cancellationToken);
}

public class DocumentMappingService : IDocumentMappingService
{
    private readonly DocumentManagementDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IIdentityAdapterClient _identityAdapterClient;
    private readonly ICatalogClient _catalogClient;
    private readonly IAuthenticationClient _authenticationClient;

    public DocumentMappingService(
        DocumentManagementDbContext dbContext,
        IMapper mapper,
        IIdentityAdapterClient identityAdapterClient,
        ICatalogClient catalogClient,
        IAuthenticationClient authenticationClient)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _identityAdapterClient = identityAdapterClient;
        _catalogClient = catalogClient;
        _authenticationClient = authenticationClient;
    }

    public async Task<List<DocumentViewModel>> MapToDocumentViewModelAsync(IList<Document> documents,
        CancellationToken cancellationToken)
    {
        var documentRelationsBag = await GetDocumentsRelationsBagAsync(documents, cancellationToken);
        return await MapDocumentsAsync<DocumentViewModel>(documents, documentRelationsBag, cancellationToken);
    }

    public async Task<List<ReportViewModel>> MapToReportViewModelAsync(IList<Document> documents, CancellationToken cancellationToken)
    {
        var documentRelationsBag = await GetDocumentsRelationsBagExtendedAsync(documents, cancellationToken);
        return await MapDocumentsAsync<ReportViewModel>(documents, documentRelationsBag, cancellationToken);
    }

    private async Task<List<TViewModel>> MapDocumentsAsync<TViewModel>(IEnumerable<Document> documents, DocumentRelationsBag documentRelationsBag, CancellationToken cancellationToken) where TViewModel : class, new()
    {
        var result = new List<TViewModel>();

        var incomingDocuments = documents
            .Where(x => x.DocumentType == DocumentType.Incoming)
            .ToList();
        result.AddRange(await MapChildDocumentAsync<IncomingDocument, TViewModel>(incomingDocuments, documentRelationsBag, cancellationToken, x => x.WorkflowHistory));

        var internalDocuments = documents
            .Where(x => x.DocumentType == DocumentType.Internal)
            .ToList();
        result.AddRange(await MapChildDocumentAsync<InternalDocument, TViewModel>(internalDocuments, documentRelationsBag, cancellationToken));

        var outogingDocuments = documents
            .Where(x => x.DocumentType == DocumentType.Outgoing)
            .ToList();
        result.AddRange(await MapChildDocumentAsync<OutgoingDocument, TViewModel>(outogingDocuments, documentRelationsBag, cancellationToken, x => x.WorkflowHistory));

        return result;
    }

    private async Task<List<TResult>> MapChildDocumentAsync<T, TResult>(IList<Document> documents, DocumentRelationsBag documentRelationsBag, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes)
        where T : VirtualDocument
        where TResult : class, new()
    {
        var documentIds = documents.Select(x => x.Id).ToList();

        var virtualDocuments = await _dbContext.Set<T>().AsQueryable()
            .Includes(includes)
            .Where(x => documentIds.Contains(x.DocumentId))
            .ToListAsync(cancellationToken);

        var documentRegistry = documents.ToDictionary(x => x, y => virtualDocuments.Where(x => x.DocumentId == y.Id));

        var result = new List<TResult>();
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
                    InternalCategories = documentRelationsBag.InternalCategories,
                    SpecialRegisterMapping = documentRelationsBag.SpecialRegisterMappings.FirstOrDefault(c => c.DocumentId == virtualDocument.DocumentId)
                };

                result.Add(_mapper.Map<VirtualDocumentAggregate<T>, TResult>(aggregate));
            }
        }

        return result;
    }

    private async Task<DocumentRelationsBag> GetDocumentsRelationsBagExtendedAsync(IList<Document> documents,
        CancellationToken cancellationToken)
    {
        var relationsBag = (DocumentRelationsBag)(await GetDocumentsRelationsBagAsync(documents, cancellationToken));
        relationsBag.SpecialRegisterMappings = await GetSpecialRegisterMappings(documents, cancellationToken);

        return relationsBag;
    }

    private async Task<List<SpecialRegisterMappingModel>> GetSpecialRegisterMappings(IList<Document> documents, CancellationToken cancellationToken)
    {
        var documentIds = documents.Select(c => c.Id).ToList();
        return await _dbContext.SpecialRegisterMappings
            .Select(c => _mapper.Map<SpecialRegisterMappingModel>(c))
            .Where(c => documentIds.Contains(c.DocumentId))
            .ToListAsync(cancellationToken);
    }

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
        public IReadOnlyList<SpecialRegisterMappingModel> SpecialRegisterMappings { get; set; }
    }
}