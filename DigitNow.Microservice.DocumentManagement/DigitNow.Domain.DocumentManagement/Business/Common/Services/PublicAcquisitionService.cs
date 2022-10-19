using DigitNow.Adapters.MS.Catalog;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.PublicAcquisitions;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.PublicAcquisitions;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IPublicAcquisitionService
    {
        // Create
        Task<PublicAcquisitionProject> AddAsync(PublicAcquisitionProject publicAcquisition, CancellationToken cancellationToken);

        // Read
        IQueryable<PublicAcquisitionProject> GetByIdQuery(long publicAcquisitionId);
        Task<List<PublicAcquisitionProject>> GetAllAsync(PublicAcquisitionFilter filter, int page, int count, CancellationToken cancellationToken);
        Task<long> CountAsync(PublicAcquisitionFilter filter, CancellationToken cancellationToken);

        // Update
        Task UpdateAsync(PublicAcquisitionProject publicAcquisition, CancellationToken cancellationToken);

    }
    public class PublicAcquisitionService : IPublicAcquisitionService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICatalogAdapterClient _catalogClient;
        private UserModel _currentUser;


        public PublicAcquisitionService(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public PublicAcquisitionService(
            DocumentManagementDbContext dbContext,
            IIdentityService identityService,
            IServiceProvider serviceProvider,
            ICatalogAdapterClient catalogClient)
        {
            _dbContext = dbContext;
            _identityService = identityService;
            _serviceProvider = serviceProvider;
            _catalogClient = catalogClient;
        }

        public IQueryable<PublicAcquisitionProject> GetByIdQuery(long publicAcquisitionId)
        {
            return _dbContext.PublicAcquisitionProjects.Where(pa => pa.Id == publicAcquisitionId);
        }

        public async Task<PublicAcquisitionProject> AddAsync(PublicAcquisitionProject publicAcquisition, CancellationToken cancellationToken)
        {
            var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

            try
            {
                _dbContext.Entry(publicAcquisition).State = EntityState.Added;
                await SetPublicAcquisitionCodeAsync(publicAcquisition, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await dbContextTransaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync(cancellationToken);
                throw;
            }
            finally
            {
                dbContextTransaction?.Dispose();
            }

            return publicAcquisition;
        }

        private async Task SetPublicAcquisitionCodeAsync(PublicAcquisitionProject publicAcquisition, CancellationToken cancellationToken)
        {
            var lastPublicAquisitionCode = await _dbContext.PublicAcquisitionProjects
                .OrderByDescending(p => p.CreatedAt)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);

            publicAcquisition.Code = $"{++lastPublicAquisitionCode}";
        }

        public async Task UpdateAsync(PublicAcquisitionProject publicAcquisition, CancellationToken cancellationToken)
        {
            await _dbContext.SingleUpdateAsync(publicAcquisition, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<PublicAcquisitionProject>> GetAllAsync(PublicAcquisitionFilter filter, int page, int count, CancellationToken cancellationToken)
        {
            var publicAcquisitionProjects = await _dbContext.PublicAcquisitionProjects
                .WhereAll((await GetPublicAcquisitionExpressions(_currentUser, filter, cancellationToken)).ToPredicates())
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * count)
                .Take(count)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return publicAcquisitionProjects;
        }

        public async Task<long> CountAsync(PublicAcquisitionFilter filter, CancellationToken cancellationToken)
        {
            _currentUser = await _identityService.GetCurrentUserAsync(cancellationToken);

            return await _dbContext.PublicAcquisitionProjects
                .WhereAll((await GetPublicAcquisitionExpressions(_currentUser, filter, cancellationToken)).ToPredicates())
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }

        private async Task<DataExpressions<PublicAcquisitionProject>> GetPublicAcquisitionExpressions(UserModel currentUser, PublicAcquisitionFilter filter, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<PublicAcquisitionProject>();

            dataExpressions.AddRange(await GetPublicAcquisitionsExpressionsAsync(filter, token));
            dataExpressions.AddRange(await GetPublicAcquisitionsUserRightsExpressionsAsync(currentUser, token));

            return dataExpressions;
        }

        private async Task<DataExpressions<PublicAcquisitionProject>> GetPublicAcquisitionsUserRightsExpressionsAsync(UserModel currentUser, CancellationToken token)
        {
            var departmentResponse = await _catalogClient.GetDepartmentByCodeAsync(UserDepartment.PublicAcquisition.Code, token);
            
            var rightsComponent = new PublicAcquisitionsPermissionsFilterComponent(_serviceProvider);
            var rightsComponentContext = new PublicAcquisitionsPermissionsFilterComponentContext
            {
                CurrentUser = currentUser,
                AllowedDepartmentId = departmentResponse.Id
            };

            return await rightsComponent.ExtractDataExpressionsAsync(rightsComponentContext, token);
        }

        private Task<DataExpressions<PublicAcquisitionProject>> GetPublicAcquisitionsExpressionsAsync(PublicAcquisitionFilter filter, CancellationToken token)
        {
            var filterComponent = new PublicAcquisitionsFilterComponent(_serviceProvider);
            var filterComponentContext = new PublicAcquisitionsFilterComponentContext
            {
                PublicAcquisitionFilter = filter
            };

            return filterComponent.ExtractDataExpressionsAsync(filterComponentContext, token);
        }
    }
}
