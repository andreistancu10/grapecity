using DigitNow.Adapters.MS.Catalog;
using DigitNow.Adapters.MS.Catalog.Poco;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Documents;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Domain.Business.Common.Factories;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services
{
    public interface IDocumentService
    {
        // Create
        Task<Document> AddAsync(Document newDocument, CancellationToken token);

        // Read
        Task<IQueryable<Document>> GetByIdQueryAsync(long documentId, CancellationToken token, bool applyPermissions = false);
        Task<IQueryable<Document>> FindByFilterQueryAsync(CancellationToken token, bool applyPermissions = false);
        Task<IQueryable<Document>> FindByRegistrationQueryAsync(long registrationNumber, int registrationYear, CancellationToken token, bool applyPermissions = false);
        Task<bool> CheckDocumentPermissionsAsync(long documentId, CancellationToken token);

        // Update
        Task SetResolutionAsync(IEnumerable<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken);

        // Delete
    }

    public class DocumentService : IDocumentService
    {
        protected readonly DocumentManagementDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICatalogAdapterClient _catalogAdapterClient;
        private readonly IIdentityService _identityService;

        public DocumentService(DocumentManagementDbContext dbContext,
            IServiceProvider serviceProvider,
            ICatalogAdapterClient catalogAdapterClient,
            IIdentityService identityService)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
            _catalogAdapterClient = catalogAdapterClient;
            _identityService = identityService;
        }

        public async Task<IQueryable<Document>> GetByIdQueryAsync(long documentId, CancellationToken token, bool applyPermissions = false)
        {
            var query = _dbContext.Documents.AsQueryable();

            if (applyPermissions)
            {
                var dataPermissionsExpressions = await GetPermissionsDataExpressionsAsync(token);

                query = query.WhereAll(dataPermissionsExpressions.ToPredicates());
            }

            return query.Where(x => x.Id == documentId);
        }        
        
        public async Task<bool> CheckDocumentPermissionsAsync(long documentId, CancellationToken token)
        {
            var query = _dbContext.Documents.Where(x => x.Id == documentId).AsQueryable();

            var dataPermissionsExpressions = await GetPermissionsDataExpressionsAsync(token);

            query = query.WhereAll(dataPermissionsExpressions.ToPredicates());

            return await query.AnyAsync(token);
        }

        public async Task<IQueryable<Document>> FindByFilterQueryAsync(CancellationToken token, bool applyPermissions = false)
        {
            var query = _dbContext.Documents.AsQueryable();

            if (applyPermissions)
            {
                var dataPermissionsExpressions = await GetPermissionsDataExpressionsAsync(token);

                query = query.WhereAll(dataPermissionsExpressions.ToPredicates());
            }

            return query;
        }

        public async Task<IQueryable<Document>> FindByRegistrationQueryAsync(long registrationNumber, int registrationYear, CancellationToken token, bool applyPermissions = false)
        {
            var query = _dbContext.Documents.AsQueryable();

            if (applyPermissions)
            {
                var dataPermissionsExpressions = await GetPermissionsDataExpressionsAsync(token);

                query = query.WhereAll(dataPermissionsExpressions.ToPredicates());
            }

            return query
                .Where(x => x.RegistrationNumber == registrationNumber && x.RegistrationDate.Year == registrationYear);
        }

        public async Task<Document> AddAsync(Document newDocument, CancellationToken token)
        {
            var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, token);
            try
            {
                // Insert the entity without relationships
                _dbContext.Entry(newDocument).State = EntityState.Added;
                await SetRegistrationNumberAsync(newDocument, token);
                await _dbContext.SaveChangesAsync(token);

                await dbContextTransaction.CommitAsync(token);
            }
            catch
            {
                //TODO: Log error
                await dbContextTransaction.RollbackAsync(token);
                throw;
            }
            finally
            {
                dbContextTransaction?.Dispose();
            }

            return newDocument;
        }

        public Task<int> CountAllAsync(Expression<Func<Document, bool>> predicate, CancellationToken token)
        {
            return _dbContext.Documents
                .Where(predicate)
                .CountAsync(token);
        }

        private async Task SetRegistrationNumberAsync(Document document, CancellationToken token)
        {
            var maxRegNumber = await _dbContext.Documents
                .Where(reg => reg.RegistrationDate.Year == DateTime.Now.Year)
                .Select(reg => reg.RegistrationNumber)
                .DefaultIfEmpty()
                .MaxAsync(token);

            document.RegistrationNumber = ++maxRegNumber;
            document.RegistrationDate = DateTime.Now;
        }

        public async Task SetResolutionAsync(IEnumerable<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken)
        {
            var dbDocuments = await _dbContext.Documents
                .Where(x => documentIds.Contains(x.Id) && x.Status != DocumentStatus.Finalized)
                .ToListAsync(cancellationToken);
            if (!dbDocuments.Any()) return;

            var foundResolutions = await _dbContext.DocumentResolutions
                    .Where(x => documentIds.Contains(x.DocumentId))
                    .ToListAsync(cancellationToken);

            foreach (var dbDocument in dbDocuments)
            {
                var foundResolution = foundResolutions.FirstOrDefault(x => x.DocumentId == dbDocument.Id);
                if (foundResolution == null)
                {
                    await _dbContext.DocumentResolutions
                        .AddAsync(DocumentResolutionFactory.Create(dbDocument, resolutionType, remarks), cancellationToken);
                }
                else
                {
                    foundResolution.ResolutionType = resolutionType;
                    foundResolution.Remarks = remarks;

                    await _dbContext.DocumentResolutions
                        .SingleUpdateAsync(foundResolution, cancellationToken);
                }


                Department departmentToReceiveDocument = null;

                if (dbDocument.DocumentType == Contracts.Documents.Enums.DocumentType.Incoming)
                {
                    departmentToReceiveDocument = await _catalogAdapterClient.GetDepartmentByCodeAsync(UserDepartment.Registry.Code, cancellationToken);
                }
                else
                {
                    departmentToReceiveDocument = await _catalogAdapterClient.GetDepartmentByIdAsync(dbDocument.SourceDestinationDepartmentId, cancellationToken);
                }

                // TODO: Create Abstract factory
                var newWorkflowResponsible = new WorkflowHistoryLog
                {
                    DocumentId = dbDocument.Id,
                    DocumentStatus = DocumentStatus.Finalized,
                    RecipientType = RecipientType.Department.Id,
                    RecipientId = departmentToReceiveDocument.Id,
                    RecipientName = $"Departamentul {departmentToReceiveDocument.Name}",
                    Resolution = (int)resolutionType
                };

                dbDocument.Status = DocumentStatus.Finalized;
                dbDocument.DestinationDepartmentId = departmentToReceiveDocument.Id;
                dbDocument.RecipientId = await _identityService.GetHeadOfDepartmentUserIdAsync(departmentToReceiveDocument.Id, cancellationToken);

                await _dbContext.WorkflowHistoryLogs.AddAsync(newWorkflowResponsible, cancellationToken);
                await _dbContext.Documents.SingleUpdateAsync(dbDocument, cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        #region [ Utils ]

        private async Task<DataExpressions<Document>> GetPermissionsDataExpressionsAsync(CancellationToken token)
        {
            var currentUser = await _identityService.GetCurrentUserAsync(token);

            var permissionComponent = new DocumentPermissionsFilterComponent(_serviceProvider);
            var permissionComponentContext = new DocumentPermissionsFilterComponentContext
            {
                CurrentUser = currentUser,
                UserPermissionsFilter = DataFilterFactory.BuildDocumentUserRightsFilter(currentUser)
            };

            return await permissionComponent.ExtractDataExpressionsAsync(permissionComponentContext, token);
        }

        #endregion
    }
}