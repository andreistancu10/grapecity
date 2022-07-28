using AutoMapper;
using DigitNow.Adapters.MS.Identity;
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;
using DigitNow.Domain.DocumentManagement.extensions.Role;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IDashboardService
    {
        Task<long> CountAllDocumentsAsync(DocumentPreprocessFilter preprocessFilter, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken);

        Task<long> CountAllArchiveDocumentsAsync(DocumentPreprocessFilter preprocessFilter, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken);

        Task<List<VirtualDocument>> GetAllDocumentsAsync(DocumentPreprocessFilter preprocessFilter, DocumentPostprocessFilter postprocessFilter, int page, int count, CancellationToken cancellationToken);
        Task<List<VirtualDocument>> GetAllArchiveDocumentsAsync(DocumentPreprocessFilter preprocessFilter, DocumentPostprocessFilter postprocessFilter, int page, int count, CancellationToken cancellationToken);
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

        private static int PreviousYear => DateTime.UtcNow.Year - 1;

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

        public Task<long> CountAllDocumentsAsync(DocumentPreprocessFilter preprocessFilter, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken)
            => CountAllDocumentsByFlowAsync(FlowType.Documents, preprocessFilter, postprocessFilter, cancellationToken);

        public Task<List<VirtualDocument>> GetAllDocumentsAsync(DocumentPreprocessFilter preprocessFilter, DocumentPostprocessFilter postprocessFilter, int page, int count, CancellationToken cancellationToken)
            => GetAllDocumentsByFlowAsync(FlowType.Documents, preprocessFilter, postprocessFilter, page, count, cancellationToken);

        public Task<long> CountAllArchiveDocumentsAsync(DocumentPreprocessFilter preprocessFilter, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken)
            => CountAllDocumentsByFlowAsync(FlowType.ArchivedDocuments, preprocessFilter, postprocessFilter, cancellationToken);

        public Task<List<VirtualDocument>> GetAllArchiveDocumentsAsync(DocumentPreprocessFilter preprocessFilter, DocumentPostprocessFilter postprocessFilter, int page, int count, CancellationToken cancellationToken)
            => GetAllDocumentsByFlowAsync(FlowType.ArchivedDocuments, preprocessFilter, postprocessFilter, page, count, cancellationToken);

        private async Task<long> CountAllDocumentsByFlowAsync(FlowType flowType, DocumentPreprocessFilter preprocessFilter, DocumentPostprocessFilter postprocessFilter, CancellationToken cancellationToken)
        {
            var documentsQuery = await BuildPreprocessDocumentsQueryAsync(flowType, preprocessFilter, cancellationToken);
            if (postprocessFilter.IsEmpty())
                return await documentsQuery.CountAsync(cancellationToken);

            var lightweightDocuments = await documentsQuery
                .Select(x => new Document { Id = x.Id, DocumentType = x.DocumentType })
                .ToListAsync(cancellationToken);

            return await _virtualDocumentService.CountVirtualDocuments(lightweightDocuments, postprocessFilter, cancellationToken);
        }

        private async Task<List<VirtualDocument>> GetAllDocumentsByFlowAsync(FlowType flowType, DocumentPreprocessFilter preprocessFilter, DocumentPostprocessFilter postprocessFilter, int page, int count, CancellationToken token)
        {
            var documentsQuery = await BuildPreprocessDocumentsQueryAsync(flowType, preprocessFilter, token);

            var documents = await documentsQuery.OrderByDescending(x => x.CreatedAt)
                 .Skip((page - 1) * count)
                 .Take(count)
                 .Select(x => new Document { Id = x.Id, DocumentType = x.DocumentType })
                 .ToListAsync(token);

            var virtualDocuments = await _virtualDocumentService.FetchVirtualDocuments(documents, postprocessFilter, token);

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
            if (userModel.HasRole(RecipientType.HeadOfDepartment))
            {
                //TODO: Get all users by departmentId
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

            var getUserByIdResponse = await _identityAdapterClient.GetUserByIdAsync(userId, cancellationToken);
            if (getUserByIdResponse == null)
                throw new InvalidOperationException($"User with identifier '{userId}' was not found!");

            return _mapper.Map<UserModel>(getUserByIdResponse);
        }

        #endregion

        #region [ Utils - Preprocessing Filters ]

        private enum FlowType
        {
            Documents,
            ArchivedDocuments
        }

        private IList<Expression<Func<Document, bool>>> GetDocumentsPreprocessBuiltinPredicates() =>
            PredicateFactory.CreatePredicatesList<Document>(x => x.CreatedAt.Year >= PreviousYear && x.IsArchived == false);

        private IList<Expression<Func<Document, bool>>> GetArchivedDocumentsPreprocessBuiltinPredicates() =>
            PredicateFactory.CreatePredicatesList<Document>(x => x.IsArchived == true);

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
        private IList<Expression<Func<Document, bool>>> GetArchivedPreprocessPredicates(DocumentPreprocessFilter preprocessFilter)
        {
            var preprocessPredicates = GetArchivedDocumentsPreprocessBuiltinPredicates();
            if (!preprocessFilter.IsEmpty())
            {
                GetDocumentsPreprocessPredicates(preprocessFilter).ForEach(predicate => preprocessPredicates.Add(predicate));
            }
            return preprocessPredicates;
        }

        private async Task<IQueryable<Document>> BuildPreprocessDocumentsQueryAsync(FlowType flowType, DocumentPreprocessFilter documentFilter, CancellationToken cancellationToken)
        {
            var documentsQuery = _dbContext.Documents.AsQueryable();
            switch (flowType)
            {
                case FlowType.Documents:
                    documentsQuery = documentsQuery.WhereAll(GetPreprocessPredicates(documentFilter));
                    break;
                case FlowType.ArchivedDocuments:
                    documentsQuery = documentsQuery.WhereAll(GetArchivedPreprocessPredicates(documentFilter));
                    break;

            }

            var userModel = await GetCurrentUserAsync(cancellationToken);

            if (!userModel.HasRole(RecipientType.Mayor))
            {
                var relatedUserIds = await GetRelatedUserIdsAsync(userModel, cancellationToken);

                // TODO:(!) This is only temporary, Apply filter permissions in the future versions                
                documentsQuery = documentsQuery
                    .Where(x => 
                        relatedUserIds.Contains(x.CreatedBy)
                        || 
                        (userModel.Departments.Contains(x.DestinationDepartmentId))
                    );
            }

            return documentsQuery;
        }

        #endregion
    }
}