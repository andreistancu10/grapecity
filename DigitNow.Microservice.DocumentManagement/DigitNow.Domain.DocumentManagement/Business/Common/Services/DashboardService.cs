using AutoMapper;
using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
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
using DigitNow.Domain.DocumentManagement.Data.EFExtensions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IDashboardService
    {
        Task<long> CountAllDocumentsAsync(IList<Expression<Func<Document, bool>>> predicate, CancellationToken cancellationToken);
        Task<List<DashboardDocumentViewModel>> GetAllDocumentsAsync(Expression<Func<Document, bool>> predicate, int skip, int take, CancellationToken cancellationToken);
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
            var user = await GetCurrentUserAsync();

            var documentsQuery = _dbContext.Documents.Where(x => true); //default(IQueryable<Document>);
            foreach (var predicate in predicates)
            {
                documentsQuery.Where(predicate);
            }            
            
            var result = default(long);
            if (user.Roles.ToList().Contains((long)UserRole.Mayor))
            {
                //TODO: Modify document service to support this

                result = await documentsQuery.CountAsync(cancellationToken);
            }
            else
            {
                var relatedUserIds = await GetRelatedUserIdsASync(user);

                //TODO: Modify document service to support this
                result = await documentsQuery
                    .Where(x => relatedUserIds.Contains(x.CreatedBy))
                    .CountAsync(cancellationToken);
            }

            return result;
        }

        public async Task<List<DashboardDocumentViewModel>> GetAllDocumentsAsync(Expression<Func<Document, bool>> predicate, int skip, int take, CancellationToken cancellationToken)
        {
            var user = await GetCurrentUserAsync();

            var documents = default(List<Document>);

            if (user.Roles.ToList().Contains((long)UserRole.Mayor))
            {
                //TODO: Modify document service to support this
                documents = await _dbContext.Documents
                    .Where(predicate)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                var relatedUserIds = await GetRelatedUserIdsASync(user);

                //TODO: Modify document service to support this
                documents = await _dbContext.Documents
                    .Where(predicate)
                    .Where(x => relatedUserIds.Contains(x.CreatedBy))
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync(cancellationToken);
            }

            return await MapDocumentsAsync(documents, cancellationToken);
        }

        private async Task<User> GetCurrentUserAsync()
        {
            var user = await _identityAdapterClient.GetUserByIdAsync(_identityService.GetCurrentUserId());
            if (user == null)
                throw new InvalidOperationException(); //TODO: Add not found exception

            return user;
        }

        private async Task<List<DashboardDocumentViewModel>> MapDocumentsAsync(IEnumerable<Document> documents, CancellationToken cancellationToken)
        {
            var result = new List<DashboardDocumentViewModel>();

            var incomingDocuments = documents
                .Where(x => x.DocumentType == DocumentType.Incoming)
                .ToList();
            result.AddRange(await MapChildDocumentAsync<IncomingDocument>(incomingDocuments, cancellationToken, x => x.WorkflowHistory));

            var internalDocuments = documents
                .Where(x => x.DocumentType == DocumentType.Internal)
                .ToList();
            result.AddRange(await MapChildDocumentAsync<InternalDocument>(internalDocuments, cancellationToken));

            var outogingDocuments = documents
                .Where(x => x.DocumentType == DocumentType.Outgoing)
                .ToList();
            result.AddRange(await MapChildDocumentAsync<OutgoingDocument>(outogingDocuments, cancellationToken, x => x.WorkflowHistory));

            return result;
        }

        private async Task<List<DashboardDocumentViewModel>> MapChildDocumentAsync<T>(List<Document> documents, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes)
            where T : VirtualDocument
        {
            var documentIds = documents.Select(x => x.Id).ToList();

            var virtualDocuments = await _dbContext.Set<T>()
                .Includes(includes)
                .Where(x => documentIds.Contains(x.Id))
                .ToListAsync(cancellationToken);

            var documentRegistry = documents.ToDictionary(x => x, y => virtualDocuments.Where(x => x.DocumentId == y.Id));

            var result = new List<DashboardDocumentViewModel>();
            foreach (var registryEntry in documentRegistry)
            {
                var document = registryEntry.Value;

                foreach (var virtualDocument in registryEntry.Value)
                {
                    result.Add(_mapper.Map<T, DashboardDocumentViewModel>(virtualDocument));
                }
            }
            return result;
        }

        private async Task<IEnumerable<long>> GetRelatedUserIdsASync(User user)
        {
            if (user.Roles.ToList().Contains((long)UserRole.Functionary))
            {
                return new List<long> { user.Id };
            }

            if (user.Roles.ToList().Contains((long)UserRole.HeadOfDepartment))
            {
                var departmentId = user.Departments.FirstOrDefault();
                var departmentUsers = await _identityAdapterClient.GetUsersByDepartmentIdAsync(departmentId);
                return departmentUsers.Users.Select(x => x.Id).ToList();
            }

            throw new InvalidOperationException(); //TODO: Add descriptive error
        }
    }
}
