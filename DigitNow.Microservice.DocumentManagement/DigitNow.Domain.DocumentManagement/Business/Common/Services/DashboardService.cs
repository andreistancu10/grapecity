using AutoMapper;
using DigitNow.Adapters.MS.Identity;
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
        Task<long> CountAllDocumentsAsync(CancellationToken cancellationToken);
        Task<long> CountAllDocumentsAsync(DocumentFilter documentFilter, CancellationToken cancellationToken);

        Task<List<Document>> GetAllDocumentsAsync(int page, int count, CancellationToken cancellationToken);
        Task<List<Document>> GetAllDocumentsAsync(DocumentFilter documentFilter, int page, int count, CancellationToken cancellationToken);
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

        public Task<long> CountAllDocumentsAsync(CancellationToken cancellationToken) =>
            CountAllInternalDocumentsAsync(GetDocumentsBuiltinPredicates(), cancellationToken);
        
        public Task<long> CountAllDocumentsAsync(DocumentFilter documentFilter, CancellationToken cancellationToken) =>
            CountAllInternalDocumentsAsync(GetJoinedPredicates(documentFilter), cancellationToken);
        

        public Task<List<Document>> GetAllDocumentsAsync(int page, int count, CancellationToken cancellationToken) =>
            GetAllInternalDocumentsAsync(GetDocumentsBuiltinPredicates(), page, count, cancellationToken);
        
        public Task<List<Document>> GetAllDocumentsAsync(DocumentFilter documentFilter, int page, int count, CancellationToken cancellationToken) =>
            GetAllInternalDocumentsAsync(GetJoinedPredicates(documentFilter), page, count, cancellationToken);
        

        #endregion

        #region [ IDashboardService Internal ]

        private async Task<long> CountAllInternalDocumentsAsync(IList<Expression<Func<Document, bool>>> predicates, CancellationToken cancellationToken)
        {
            var documentsCountQuery = _dbContext.Documents
                .WhereAll(predicates);

            var userModel = await GetCurrentUserAsync(cancellationToken);

            if (!IsRole(userModel, UserRole.Mayor))
            {
                var relatedUserIds = await GetRelatedUserIdsAsync(userModel, cancellationToken);

                documentsCountQuery = documentsCountQuery
                    .Where(x => relatedUserIds.Contains(x.CreatedBy));
            }

            return await documentsCountQuery.CountAsync(cancellationToken);
        }

        private async Task<List<Document>> GetAllInternalDocumentsAsync(IList<Expression<Func<Document, bool>>> predicates, int page, int count, CancellationToken cancellationToken)
        {
            var documentsQuery = _dbContext.Documents
                .WhereAll(predicates);

            var userModel = await GetCurrentUserAsync(cancellationToken);

            if (!IsRole(userModel, UserRole.Mayor))
            {
                var relatedUserIds = await GetRelatedUserIdsAsync(userModel, cancellationToken);

                documentsQuery = documentsQuery
                    .Where(x => relatedUserIds.Contains(x.CreatedBy));
            }

            return await documentsQuery.OrderByDescending(x => x.RegistrationDate)
                 .Skip((page - 1) * count)
                 .Take(count)
                 .ToListAsync(cancellationToken);
        }

        #endregion

        #region [ Utils - Predicates ]

        private IList<Expression<Func<Document, bool>>> GetDocumentsBuiltinPredicates() =>        
            PredicateFactory.CreatePredicatesList<Document>(x => x.CreatedAt.Year >= PreviousYear);        

        private IList<Expression<Func<Document, bool>>> GetDocumentsCustomPredicates(DocumentFilter documentFilter) =>
            ExpressionFilterBuilderRegistry.GetDocumentPredicatesByFilter(documentFilter).Build();

        private IList<Expression<Func<Document, bool>>> GetJoinedPredicates(DocumentFilter documentFilter = null)
        {
            var joinedPredicates = GetDocumentsBuiltinPredicates();
            if (documentFilter != null)
            {
                GetDocumentsCustomPredicates(documentFilter).ForEach(predicate => joinedPredicates.Add(predicate));
            }
            return joinedPredicates;
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

        #region [ Utils ]

        private bool IsRole(UserModel userModel, UserRole role) =>
            userModel.Roles.Contains((long)role);

        #endregion
    }
}
