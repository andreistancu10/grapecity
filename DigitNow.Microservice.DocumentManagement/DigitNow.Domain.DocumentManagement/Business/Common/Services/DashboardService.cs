using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Data.Filters.Documents;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Documents;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IDashboardService
    {
        Task<long> CountActiveDocumentsAsync(DocumentFilter filter, CancellationToken token);
        Task<List<VirtualDocument>> GetActiveDocumentsAsync(DocumentFilter filter, int page, int count, CancellationToken token);

        Task<long> CountArchivedDocumentsAsync(DocumentFilter filter, CancellationToken token);
        Task<List<VirtualDocument>> GetArchivedDocumentsAsync(DocumentFilter filter, int page, int count, CancellationToken token);
    }

    public class DashboardService : IDashboardService
    {
        #region [ Fields ]

        private readonly DocumentManagementDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly IIdentityService _identityService;
        private readonly IVirtualDocumentService _virtualDocumentService;

        #endregion

        #region [ Construction ]

        public DashboardService(DocumentManagementDbContext dbContext,
            IServiceProvider serviceProvider,
            IIdentityService identityService,
            IVirtualDocumentService virtualDocumentService)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
            _identityService = identityService;
            _virtualDocumentService = virtualDocumentService;
        }

        #endregion

        #region [ IDashboardService ]

        public async Task<long> CountActiveDocumentsAsync(DocumentFilter filter, CancellationToken token)
        {
            var currentUser = await _identityService.GetCurrentUserAsync(token);

            return await GetBuiltinDocumentsQuery()
                .WhereAll((await GetActiveDocumentsExpressions(currentUser, filter, token)).ToPredicates())
                .CountAsync(token);
        }

        public async Task<List<VirtualDocument>> GetActiveDocumentsAsync(DocumentFilter filter, int page, int count, CancellationToken token)
        {
            var currentUser = await _identityService.GetCurrentUserAsync(token);

            var documents = await GetBuiltinDocumentsQuery()
                 .WhereAll((await GetActiveDocumentsExpressions(currentUser, filter, token)).ToPredicates())
                 .Skip((page - 1) * count)
                 .Take(count)
                 .ToListAsync(token);

            return _virtualDocumentService.ConvertDocumentsToVirtualDocuments(documents)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }

        public async Task<long> CountArchivedDocumentsAsync(DocumentFilter filter, CancellationToken token)
        {
            var currentUser = await _identityService.GetCurrentUserAsync(token);

            return await GetBuiltinDocumentsQuery()
                .WhereAll((await GetArchivedDocumentsExpressions(currentUser, filter, token)).ToPredicates())
                .CountAsync(token);
        }

        public async Task<List<VirtualDocument>> GetArchivedDocumentsAsync(DocumentFilter filter, int page, int count, CancellationToken token)
        {
            var currentUser = await _identityService.GetCurrentUserAsync(token);

            var documents = await GetBuiltinDocumentsQuery()
                 .WhereAll((await GetArchivedDocumentsExpressions(currentUser, filter, token)).ToPredicates())
                 .Skip((page - 1) * count)
                 .Take(count)
                 .ToListAsync(token);

            return _virtualDocumentService.ConvertDocumentsToVirtualDocuments(documents)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }

        #endregion

        #region [ ActiveDocuments - Filters ]

        private async Task<DataExpressions<Document>> GetActiveDocumentsExpressions(UserModel currentUser, DocumentFilter filter, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<Document>();

            dataExpressions.AddRange(await GetActiveDocumentsExpressionsAsync(filter, token));
            dataExpressions.AddRange(await GetDocumentsUserRightsExpressionsAsync(currentUser, token));

            return dataExpressions;
        }

        private Task<DataExpressions<Document>> GetActiveDocumentsExpressionsAsync(DocumentFilter filter, CancellationToken token)
        {
            var filterComponent = new ActiveDocumentsFilterComponent(_serviceProvider);
            var filterComponentContext = new ActiveDocumentsFilterComponentContext
            {
                DocumentFilter = filter
            };

            return filterComponent.ExtractDataExpressionsAsync(filterComponentContext, token);
        }

        #endregion

        #region [ ArchivedDocuments - Filters ]

        private async Task<DataExpressions<Document>> GetArchivedDocumentsExpressions(UserModel currentUser, DocumentFilter filter, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<Document>();

            dataExpressions.AddRange(await GetArchivedDocumentsExpressionsAsync(filter, token));
            dataExpressions.AddRange(await GetDocumentsUserRightsExpressionsAsync(currentUser, token));

            return dataExpressions;
        }

        private Task<DataExpressions<Document>> GetArchivedDocumentsExpressionsAsync(DocumentFilter filter, CancellationToken token)
        {
            var filterComponent = new ArchivedDocumentsFilterComponent(_serviceProvider);
            var filterComponentContext = new ArchivedDocumentsFilterComponentContext
            {
                DocumentFilter = filter
            };

            return filterComponent.ExtractDataExpressionsAsync(filterComponentContext, token);
        }

        #endregion

        #region [ Shared - Filters ]

        private Task<DataExpressions<Document>> GetDocumentsUserRightsExpressionsAsync(UserModel currentUser, CancellationToken token)
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

        #region [ Helpers ]

        private IQueryable<Document> GetBuiltinDocumentsQuery()
        {
            return _dbContext.Documents
                 .Include(x => x.IncomingDocument)
                 .Include(x => x.InternalDocument)
                 .Include(x => x.OutgoingDocument);
        }

        #endregion
    }
}
