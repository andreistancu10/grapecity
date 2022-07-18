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
        private readonly IVirtualDocumentService _virtualDocumentService;

        #endregion

        #region [ Properties ]

        private int PreviousYear => DateTime.UtcNow.Year - 1;

        #endregion

        #region [ Construction ]

        public DashboardService(DocumentManagementDbContext dbContext,
            IMapper mapper,
            IIdentityService identityService,
            IIdentityAdapterClient identityAdapterClient,
            IAuthenticationClient identityManager,
            IVirtualDocumentService virtualDocumentService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _identityService = identityService;
            _identityAdapterClient = identityAdapterClient;
            _authenticationClient = identityManager;
            _virtualDocumentService = virtualDocumentService;
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

            return await _virtualDocumentService.CountVirtualDocuments(lightweightDocuments, postprocessFilter, cancellationToken);
        }

        public async Task<List<VirtualDocument>> GetAllDocumentsAsync(DocumentPreprocessFilter preprocessFilter, DocumentPostprocessFilter postprocessFilter, int page, int count, CancellationToken cancellationToken)
        {
            var documentsQuery = await BuildPreprocessDocumentsQueryAsync(preprocessFilter, cancellationToken);

            var documents = await documentsQuery.OrderByDescending(x => x.CreatedAt)
                 .Skip((page - 1) * count)
                 .Take(count)
                 .Select(x => new Document { Id = x.Id, DocumentType = x.DocumentType })
                 .ToListAsync(cancellationToken);

            var virtualDocuments = await _virtualDocumentService.FetchVirtualDocuments(documents, postprocessFilter, cancellationToken);

            return virtualDocuments
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
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
            ExpressionFilterBuilderRegistry.GetDocumentPreprocessFilterBuilder(_dbContext, preprocessFilter).Build();

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

        #region [ Utils ]

        private bool IsRole(UserModel userModel, UserRole role) =>
            userModel.Roles.Contains(role.Code);

        #endregion
    }
}
