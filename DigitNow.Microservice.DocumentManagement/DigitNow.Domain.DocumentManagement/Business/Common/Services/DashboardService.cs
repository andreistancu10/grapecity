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
        Task<List<Document>> GetAllDocumentsAsync(DocumentFilter documentFilter, int page, int count, CancellationToken cancellationToken, params Expression<Func<Document, bool>>[] predicates);
    }

    public class DashboardService : IDashboardService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IIdentityAdapterClient _identityAdapterClient;
        private readonly ICatalogClient _catalogClient;
        private readonly IAuthenticationClient _authenticationClient;
        private int PreviousYear => DateTime.UtcNow.Year - 1;

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

        public async Task<long> CountAllDocumentsAsync(CancellationToken cancellationToken)
        {
            var predicates = PredicateFactory.CreatePredicatesList<Document>(x => x.CreatedAt.Year >= PreviousYear);

            return await CountAllInternalDocumentsAsync(predicates, cancellationToken);
        }

        public async Task<long> CountAllDocumentsAsync(DocumentFilter documentFilter, CancellationToken cancellationToken)
        {
            var predicates = ExpressionFilterBuilderRegistry
                .GetDocumentPredicatesByFilter(documentFilter)
                .Build();

            return await CountAllInternalDocumentsAsync(predicates, cancellationToken);

        }

        private async Task<long> CountAllInternalDocumentsAsync(IList<Expression<Func<Document, bool>>> predicates, CancellationToken cancellationToken)
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
                var relatedUserIds = await GetRelatedUserIdsASync(userModel, cancellationToken);

                documentsCountQuery = _dbContext.Documents
                    .WhereAll(predicates)
                    .Where(x => relatedUserIds.Contains(x.CreatedBy));
            }

            var result = await documentsCountQuery.CountAsync(cancellationToken);

            return result;
        }

        public async Task<List<Document>> GetAllDocumentsAsync(int page, int count, CancellationToken cancellationToken)
        {
            var predicates = PredicateFactory.CreatePredicatesList<Document>(x => x.CreatedAt.Year >= PreviousYear);

            return await GetAllInternalDocumentsAsync(predicates, page, count, cancellationToken);
        }

        public async Task<List<Document>> GetAllDocumentsAsync(DocumentFilter documentFilter, int page, int count, CancellationToken cancellationToken, params Expression<Func<Document, bool>>[] predicates)
        {
            var allPredicates = ExpressionFilterBuilderRegistry
                .GetDocumentPredicatesByFilter(documentFilter)
                .Build().ToList();

            if (predicates.Any())
            {
                allPredicates.AddRange(predicates);
            }

            return await GetAllInternalDocumentsAsync(allPredicates, page, count, cancellationToken);
        }

        private async Task<List<Document>> GetAllInternalDocumentsAsync(IList<Expression<Func<Document, bool>>> predicates, int page, int count, CancellationToken cancellationToken)
        {
            var userModel = await GetCurrentUserAsync(cancellationToken);

            var documentsQuery = _dbContext.Documents
                .WhereAll(predicates);

            if (!userModel.Roles.Contains((long)UserRole.Mayor))
            {
                var relatedUserIds = await GetRelatedUserIdsASync(userModel, cancellationToken);

                documentsQuery = documentsQuery.Where(x => relatedUserIds.Contains(x.CreatedBy));
            }

            return await documentsQuery.OrderByDescending(x => x.RegistrationDate)
                 .Skip((page - 1) * count)
                 .Take(count)
                 .ToListAsync(cancellationToken);
        }

        private async Task<IEnumerable<long>> GetRelatedUserIdsASync(UserModel userModel, CancellationToken cancellationToken) =>
            (await GetRelatedUsersAsync(userModel, cancellationToken)).Select(x => x.Id);

        private async Task<IList<UserModel>> GetRelatedUsersAsync(UserModel userModel, CancellationToken cancellationToken)
        {
            if (userModel.Roles.ToList().Contains((long)UserRole.HeadOfDepartment))
            {
                // Note: This should be refactored along with adapters
                var departmentId = userModel.Departments.FirstOrDefault();
                var departmentUsers = await _identityAdapterClient.GetUsersByDepartmentIdAsync(departmentId, cancellationToken);

                return departmentUsers.Users
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
    }
}
