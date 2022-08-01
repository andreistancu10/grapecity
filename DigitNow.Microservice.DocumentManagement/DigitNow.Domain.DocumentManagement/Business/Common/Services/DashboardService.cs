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
using System.Linq.Expressions;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Data.Filters.Documents.Preprocess;
using DigitNow.Domain.DocumentManagement.Data.Filters.Documents.Postprocess;
using DigitNow.Domain.DocumentManagement.Data.Filters.Documents;
using DigitNow.Domain.DocumentManagement.Data.Filters.DocumentsRights.Preprocess;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Preprocess;
using DigitNow.Domain.DocumentManagement.Data.Filters;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IDashboardService
    {
        Task<long> CountActiveDocumentsAsync(DocumentFilter filter, CancellationToken token);

        Task<List<VirtualDocument>> GetActiveDocumentsAsync(DocumentFilter filter, int page, int count, CancellationToken token);
    }

    public class DashboardService : IDashboardService
    {
        #region [ Fields ]

        private readonly DocumentManagementDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IIdentityAdapterClient _identityAdapterClient;
        private readonly IAuthenticationClient _authenticationClient;
        private readonly IVirtualDocumentService _virtualDocumentService;
        private readonly IDocumentService _documentService;

        #endregion

        #region [ Properties ]

        private static int PreviousYear => DateTime.UtcNow.Year - 1;

        #endregion

        #region [ Construction ]

        public DashboardService(DocumentManagementDbContext dbContext,
            IServiceProvider serviceProvider,
            IMapper mapper,
            IIdentityService identityService,
            IIdentityAdapterClient identityAdapterClient,
            IAuthenticationClient identityManager,
            IVirtualDocumentService virtualDocumentService,
            IDocumentService documentService)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _identityService = identityService;
            _identityAdapterClient = identityAdapterClient;
            _authenticationClient = identityManager;
            _virtualDocumentService = virtualDocumentService;
            _documentService = documentService;
        }

        #endregion

        #region [ IDashboardService ]

        public async Task<long> CountActiveDocumentsAsync(DocumentPreprocessFilter preprocessFilter, DocumentPostprocessFilter postprocessFilter, CancellationToken token)
        {
            var documentsQuery = await BuildPreprocessDocumentsQueryAsync(preprocessFilter, token);
            if (postprocessFilter.IsEmpty())
                return await documentsQuery.CountAsync(token);

            var lightweightDocuments = await documentsQuery
                .Select(x => new Document { Id = x.Id, DocumentType = x.DocumentType })
                .ToListAsync(token);

            return await _virtualDocumentService.CountVirtualDocuments(lightweightDocuments, postprocessFilter, token);
        }

        public async Task<List<VirtualDocument>> GetActiveDocumentsAsync(DocumentFilter filter, int page, int count, CancellationToken token)
        {
            var currentUser = await GetCurrentUserAsync(token);

            // 10 elements
            var documents = await _dbContext.Documents
                 .Include(x => x.IncomingDocument)
                 .Include(x => x.InternalDocument)
                 .Include(x => x.OutgoingDocument)

                 .WhereAll((await GetActiveDocumentsExpressions(currentUser, filter, token)).ToPredicates())
                 .Skip((page - 1) * count)
                 .Take(count)
                 .Select(x => new Document { Id = x.Id, DocumentType = x.DocumentType })
                 .ToListAsync(token);

            // 7 elements
            var virtualDocuments = await _virtualDocumentService.FetchVirtualDocuments(documents, filter.PostProcessFilter, token);

            // relationships

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
            if (IsRole(userModel, RecipientType.HeadOfDepartment))
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

        #region [ ActiveDocuments - Filters ]

        private async Task<DataExpressions<Document>> GetActiveDocumentsExpressions(UserModel currentUser, DocumentFilter filter, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<Document>();

            dataExpressions.AddRange(await GetActiveDocumentsPreprocessExpressionsAsync(filter, token));
            dataExpressions.AddRange(await GetActiveDocumentsPreprocessUserRightsExpressionsAsync(currentUser, token));

            return dataExpressions;
        }

        private Task<DataExpressions<Document>> GetActiveDocumentsPreprocessExpressionsAsync(DocumentFilter filter, CancellationToken token)
        {
            var filterComponent = new ActiveDocumentsPreprocessFilterComponent(_serviceProvider);
            var filterComponentContext = new ActiveDocumentsPreprocessFilterComponentContext
            {
                DocumentFilter = filter
            };

            return filterComponent.ExtractDataExpressionsAsync(filterComponentContext, token);
        }

        private Task<DataExpressions<Document>> GetActiveDocumentsPreprocessUserRightsExpressionsAsync(UserModel currentUser, CancellationToken token)
        {
            var rightsComponent = new DocumentRightsFilterPreprocessComponent(_serviceProvider);
            var rightsComponentContext = new DocumentRightsFilterPreprocessComponentContext
            {
                CurrentUser = currentUser,
                DepartmentRightsFilter = DataFilterFactory.BuildDocumentDepartmentRightsFilter(currentUser),
                UserRightsFilter = DataFilterFactory.BuildDocumentUserRightsFilter(currentUser)
            };

            return rightsComponent.ExtractDataExpressionsAsync(rightsComponentContext, token);
        }


        #endregion 

        #region [ Utils - Preprocessing Filters ]

        //private IList<Expression<Func<Document, bool>>> GetDocumentsPreprocessBuiltinPredicates() =>
        //    PredicateFactory.CreatePredicatesList<Document>(x => x.CreatedAt.Year >= PreviousYear);

        //private IList<Expression<Func<Document, bool>>> GetDocumentsPreprocessPredicates(DocumentPreprocessFilter preprocessFilter) =>
        //    ExpressionFilterBuilderRegistry.GetDocumentPreprocessFilterBuilder(_dbContext, preprocessFilter).Build();

        //private IList<Expression<Func<Document, bool>>> GetPreprocessPredicates(DocumentPreprocessFilter preprocessFilter)
        //{
        //    var preprocessPredicates = GetDocumentsPreprocessBuiltinPredicates();
        //    if (!preprocessFilter.IsEmpty())
        //    {
        //        GetDocumentsPreprocessPredicates(preprocessFilter).ForEach(predicate => preprocessPredicates.Add(predicate));
        //    }
        //    return preprocessPredicates;
        //}

        //private async Task<IQueryable<Document>> BuildPreprocessDocumentsQueryAsync(DocumentPreprocessFilter documentPreprocessFilter, CancellationToken cancellationToken)
        //{
        //    var documentsQuery = _dbContext.Documents
        //        .WhereAll(GetPreprocessPredicates(documentPreprocessFilter));

        //    var userModel = await GetCurrentUserAsync(cancellationToken);

        //    var registryOfficeDepartmentId = default(long); //TODO: Add this
        //    var containsRegistryOfficeDepartments = userModel.Departments.Contains(registryOfficeDepartmentId);
        //    if (containsRegistryOfficeDepartments)
        //    {
        //        documentsQuery.WhereAll(GetDocumentDepartmentRightsPredicates(userModel));
        //    }
        //    else
        //    {
        //        documentsQuery.WhereAll(GetDocumentUserRightsPredicates(userModel));
        //    }

        //    //if (!IsRole(userModel, RecipientType.Mayor))
        //    //{
        //    //    var relatedUserIds = await GetRelatedUserIdsAsync(userModel, cancellationToken);

        //    //    // TODO:(!) This is only temporary, Apply filter permissions in the future versions                
        //    //    documentsQuery = documentsQuery
        //    //        .Where(x => 
        //    //            relatedUserIds.Contains(x.CreatedBy)
        //    //            || 
        //    //            (userModel.Departments.Contains(x.DestinationDepartmentId))
        //    //        );
        //    //}

        //    return documentsQuery;
        //}

        #endregion

        #region [ Utils ]

        private static bool IsRole(UserModel userModel, RecipientType role) =>
            userModel.Roles.Contains(role.Code);

        #endregion
    }
}
