using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IDocumentMappingService
    {
        Task<List<DocumentViewModel>> MapToDocumentViewModelAsync(IList<VirtualDocument> documents, CancellationToken cancellationToken);
        Task<List<ReportViewModel>> MapToReportViewModelAsync(IList<VirtualDocument> documents, CancellationToken cancellationToken);
    }

    public class DocumentMappingService : IDocumentMappingService
    {
        #region [ Fields ]

        private readonly DocumentRelationsFetcher _documentRelationsFetcher;
        private readonly IMapper _mapper;
        
        #endregion

        #region [ Construction ]

        public DocumentMappingService(
            IServiceProvider serviceProvider,
            IMapper mapper)
        {
            _mapper = mapper;
            _documentRelationsFetcher = new DocumentRelationsFetcher(serviceProvider);
        }

        #endregion

        #region [ IDocumentMappingService ]

        public async Task<List<DocumentViewModel>> MapToDocumentViewModelAsync(IList<VirtualDocument> virtualDocuments, CancellationToken cancellationToken)
        {
            await _documentRelationsFetcher.FetchRelationshipsAsync(new DocumentsFetcherContext { Documents = virtualDocuments }, cancellationToken);
            return MapDocuments<DocumentViewModel>(virtualDocuments)
                .OrderByDescending(x => x.RegistrationDate)
                .ToList();
        }

        public async Task<List<ReportViewModel>> MapToReportViewModelAsync(IList<VirtualDocument> virtualDocuments, CancellationToken cancellationToken)
        {
            await _documentRelationsFetcher.FetchRelationshipsAsync(new DocumentsFetcherContext { Documents = virtualDocuments }, cancellationToken);
            return MapDocuments<ReportViewModel>(virtualDocuments)
                .OrderByDescending(x => x.RegistrationDate)
                .ToList();
        }

        #endregion

        #region [ Helpers ]

        private List<TViewModel> MapDocuments<TViewModel>(IList<VirtualDocument> documents)
            where TViewModel : class, new()
        {
            var result = new List<TViewModel>();

            var incomingDocuments = documents.Where(x => x is IncomingDocument).Cast<IncomingDocument>();
            if (incomingDocuments.Any())
            {
                result.AddRange(MapChildDocuments<IncomingDocument, TViewModel>(incomingDocuments));
            }

            var internalDocuments = documents.Where(x => x is InternalDocument).Cast<InternalDocument>();
            if (internalDocuments.Any())
            {
                result.AddRange(MapChildDocuments<InternalDocument, TViewModel>(internalDocuments));
            }

            var outgoingDocuments = documents.Where(x => x is OutgoingDocument).Cast<OutgoingDocument>();
            if (outgoingDocuments.Any())
            {
                result.AddRange(MapChildDocuments<OutgoingDocument, TViewModel>(outgoingDocuments));
            }

            return result;
        }

        private List<TResult> MapChildDocuments<T, TResult>(IEnumerable<T> childDocuments)
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
                    VirtualDocument = virtualDocument,
                    Users = documentRelationsBag.Users,
                    Categories = documentRelationsBag.Categories,
                    InternalCategories = documentRelationsBag.InternalCategories,
                    Departments = documentRelationsBag.Departments,
                    SpecialRegisterMapping = documentRelationsBag.SpecialRegisterMappings.FirstOrDefault(c => c.DocumentId == virtualDocument.DocumentId)
                };

                result.Add(_mapper.Map<VirtualDocumentAggregate<T>, TResult>(aggregate));
            }
            return result;
        }

    private async Task<List<SpecialRegisterMappingModel>> GetSpecialRegisterMappings(IList<Document> documents, CancellationToken cancellationToken)
    {
        var documentIds = documents.Select(c => c.Id).ToList();
        var specialRegisterMappings = await _dbContext.SpecialRegisterMappings
            .Where(c => documentIds.Contains(c.DocumentId))
        .ToListAsync(cancellationToken);

        return specialRegisterMappings.Select(c => _mapper.Map<SpecialRegisterMappingModel>(c)).ToList();
    }

    private async Task<List<DepartmentModel>> GetDepartmentsAsync(CancellationToken cancellationToken)
    {
        var departmentsResponse = await _catalogClient.Departments.GetDepartmentsAsync(cancellationToken);

        var departmentModel = departmentsResponse.Departments
            .Select(x => _mapper.Map<DepartmentModel>(x))
            .ToList();

        return departmentModel;
    }

    private async Task<DocumentRelationsBag> GetDocumentsRelationsBagAsync(IList<Document> documents, CancellationToken cancellationToken)
    {
        var users = await GetRelatedUserRegistryAsync(documents, cancellationToken);
        var documentCategories = await GetDocumentCategoriesAsync(cancellationToken);
        var internalDocumentCategories = await GetInternalDocumentCategoriesAsync(cancellationToken);
        var specialRegisterMappings = await GetSpecialRegisterMappings(documents, cancellationToken);
        var departments = await GetDepartmentsAsync(cancellationToken);

        return new DocumentRelationsBag
        {
            Users = users,
            Categories = documentCategories,
            InternalCategories = internalDocumentCategories,
            SpecialRegisterMappings = specialRegisterMappings,
            Departments = departments
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
        public IReadOnlyList<DepartmentModel> Departments { get; set; }
    }
}