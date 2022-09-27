using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.Risks.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Scim;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Risks;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using DigitNow.Domain.DocumentManagement.Data.Filters.Risks;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using Microsoft.EntityFrameworkCore;
using System.Data;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Documents;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Risks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IRiskService : IScimStateService
    {
        // Create
        Task<Risk> AddAsync(Risk risk, CancellationToken cancellationToken);
        Task<RiskTrackingReport> CreateRiskTrackingReportAsync(RiskTrackingReport riskTrackingReport, CancellationToken cancellationToken);

        // Read
        IQueryable<Risk> GetByIdQuery(long riskId);
        Task<List<Risk>> GetAllAsync(RiskFilter filter, int page, int count, CancellationToken cancellationToken);
        Task<long> CountAsync(RiskFilter filter, CancellationToken cancellationToken);

        // Update
        Task UpdateAsync(Risk risk, CancellationToken cancellationToken);
    }

    public class RiskService : ScimStateService, IRiskService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICatalogClient _catalogClient;
        private UserModel _currentUser;

        public RiskService(
            DocumentManagementDbContext dbContext, 
            IIdentityService identityService, 
            IServiceProvider serviceProvider,
            ICatalogClient catalogClient) : base(dbContext)
        {
            _dbContext = dbContext;
            _identityService = identityService;
            _serviceProvider = serviceProvider;
            _catalogClient = catalogClient;
        }

        public IQueryable<Risk> GetByIdQuery(long riskId)
        {
            return _dbContext.Risks.Where(risk => risk.Id == riskId);
        }

        public async Task<Risk> AddAsync(Risk risk, CancellationToken cancellationToken)
        {
            var scimStates = await _catalogClient.ScimStates.GetScimStatesAsync(cancellationToken);
            risk.StateId = scimStates.ScimStates.FirstOrDefault(c => c.Name.ToLower() == ScimState.Active.ToString().ToLower()).Id;

            risk.RiskExposureEvaluation =
                CalculateRiskExposureEvaluation(risk.ProbabilityOfApparitionEstimation, risk.ImpactOfObjectivesEstimation);
            var dbContextTransaction = await DbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            try
            {
                DbContext.Entry(risk).State = EntityState.Added;
                await SetRiskCodeAsync(risk, cancellationToken);
                await DbContext.SaveChangesAsync(cancellationToken);
                await dbContextTransaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await dbContextTransaction.RollbackAsync(cancellationToken);
                throw;
            }
            finally
            {
                dbContextTransaction?.Dispose();
            }

            return risk;
        }

        public async Task UpdateAsync(Risk risk, CancellationToken cancellationToken)
        {
            await ChangeStateAsync(new List<long> { risk.Id }, ScimEntity.ScimRisk, risk.StateId, cancellationToken);
            await DbContext.SingleUpdateAsync(risk, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task SetRiskCodeAsync(Risk risk, CancellationToken token)
        {
            var lastRiskId = await DbContext.Risks
                .Where(item => item.CreatedAt.Year == DateTime.UtcNow.Year)
                .OrderByDescending(p => p.CreatedAt)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(token);

            risk.Code = $"RISK{DateTime.Now.Year}_{++lastRiskId}";
        }

        public static string CalculateRiskExposureEvaluation(RiskProbability probability, RiskProbability impact)
        {
            return (probability, impact) switch
            {
                (RiskProbability.Low, RiskProbability.Low) => RiskExposure.SS,
                (RiskProbability.Low, RiskProbability.Medium) => RiskExposure.SM,
                (RiskProbability.Low, RiskProbability.High) => RiskExposure.SR,
                (RiskProbability.Medium, RiskProbability.Low) => RiskExposure.MS,
                (RiskProbability.Medium, RiskProbability.Medium) => RiskExposure.MM,
                (RiskProbability.Medium, RiskProbability.High) => RiskExposure.MR,
                (RiskProbability.High, RiskProbability.Low) => RiskExposure.RS,
                (RiskProbability.High, RiskProbability.Medium) => RiskExposure.RM,
                (RiskProbability.High, RiskProbability.High) => RiskExposure.RR,
                _ => null,
            };
        }

        public async Task<long> CountAsync(RiskFilter filter, CancellationToken cancellationToken)
        {
            _currentUser = await _identityService.GetCurrentUserAsync(cancellationToken);
            
            return await _dbContext.Risks
                .WhereAll((await GetRisksExpressions(_currentUser, filter, cancellationToken)).ToPredicates())
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }

        private async Task<DataExpressions<Risk>> GetRisksExpressions(UserModel currentUser, RiskFilter filter, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<Risk>();

            dataExpressions.AddRange(await GetRisksExpressionsAsync(filter, token));
            dataExpressions.AddRange(await GetRisksUserRightsExpressionsAsync(currentUser, token));

            return dataExpressions;
        }

        private Task<DataExpressions<Risk>> GetRisksUserRightsExpressionsAsync(UserModel currentUser, CancellationToken token)
        {
            var rightsComponent = new RisksPermissionsFilterComponent(_serviceProvider);
            var rightsComponentContext = new RisksPermissionsFilterComponentContext
            {
                CurrentUser = currentUser
            };

            return rightsComponent.ExtractDataExpressionsAsync(rightsComponentContext, token);
        }

        private Task<DataExpressions<Risk>> GetRisksExpressionsAsync(RiskFilter filter, CancellationToken token)
        {
            var filterComponent = new RisksFilterComponent(_serviceProvider);
            var filterComponentContext = new RisksFilterComponentContext
            {
                RiskFilter = filter
            };

            return filterComponent.ExtractDataExpressionsAsync(filterComponentContext, token);
        }

        public async Task<List<Risk>> GetAllAsync(RiskFilter filter, int page, int count, CancellationToken cancellationToken)
        {
            var risks = await _dbContext.Risks
                .WhereAll((await GetRisksExpressions(_currentUser, filter, cancellationToken)).ToPredicates())
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * count)
                .Take(count)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return risks;
        }

        public async Task<RiskTrackingReport> CreateRiskTrackingReportAsync(RiskTrackingReport riskTrackingReport, CancellationToken cancellationToken)
        {
            await _dbContext.RiskTrackingReports.AddAsync(riskTrackingReport, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return riskTrackingReport;
        }
    }
}
