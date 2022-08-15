using DigitNow.Adapters.MS.Catalog;
using DigitNow.Adapters.MS.Catalog.Poco;
using DigitNow.Adapters.MS.Identity;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using DigitNow.Domain.DocumentManagement.Domain.Business.Common.Factories;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services
{
    public interface IDocumentService
    {
        Task<Document> AddAsync(Document newDocument, CancellationToken token);
        Task<Document> FindAsync(Expression<Func<Document, bool>> predicate, CancellationToken token, params Expression<Func<Document, object>>[] includes);
        Task<List<Document>> FindAllAsync(Expression<Func<Document, bool>> predicate, CancellationToken token);

        IQueryable<Document> FindAllQueryable(Expression<Func<Document, bool>> predicate);
        Task<int> CountAllAsync(Expression<Func<Document, bool>> predicate, CancellationToken token);
        Task SetResolutionAsync(IEnumerable<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken);
    }

    public class DocumentService : IDocumentService
    {
        protected readonly DocumentManagementDbContext _dbContext;
        private readonly ICatalogAdapterClient _catalogAdapterClient;
        private readonly IIdentityService _identityService;

        public DocumentService(DocumentManagementDbContext dbContext, ICatalogAdapterClient catalogAdapterClient, IIdentityService identityService)
        {
            _dbContext = dbContext;
            _catalogAdapterClient = catalogAdapterClient;
            _identityService = identityService;
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

        public Task<Document> FindAsync(Expression<Func<Document, bool>> predicate, CancellationToken token, params Expression<Func<Document, object>>[] includes)
        {
            return _dbContext.Documents
                .Includes(includes)
                .FirstOrDefaultAsync(predicate, token);
        }

        public Task<List<Document>> FindAllAsync(Expression<Func<Document, bool>> predicate, CancellationToken token)
        {
            return _dbContext.Documents
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync(token);
        }

        public IQueryable<Document> FindAllQueryable(Expression<Func<Document, bool>> predicate)
        {
            return _dbContext.Documents
                .Where(predicate);
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
    }
}