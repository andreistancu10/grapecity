using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.RiskTrackingReports;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Risks;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IRiskTrackingReportService
    {
        // Create
        Task<RiskTrackingReport> CreateRiskTrackingReportAsync(RiskTrackingReport riskTrackingReport, CancellationToken cancellationToken);

        // GET
        IQueryable<RiskTrackingReport> GetByRiskIdQuery(long riskId);
        Task<List<RiskTrackingReport>> GetAllAsync(long riskId, int page, int count, CancellationToken cancellationToken);
        Task<long> CountAsync(long riskId, CancellationToken cancellationToken);
    }
    public class RiskTrackingReportService : IRiskTrackingReportService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IServiceProvider _serviceProvider;
        private UserModel _currentUser;

        public RiskTrackingReportService(DocumentManagementDbContext dbContext, IIdentityService identityService, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _identityService = identityService;
            _serviceProvider = serviceProvider;
        }

        public async Task<List<RiskTrackingReport>> GetAllAsync(long riskId, int page, int count, CancellationToken cancellationToken)
        {
            var risks = await GetByRiskIdQuery(riskId)
                .WhereAll((await GetRiskTrackingReportsExpressions(_currentUser, cancellationToken)).ToPredicates())
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * count)
                .Take(count)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return risks;
        }

        public async Task<long> CountAsync(long riskId, CancellationToken cancellationToken)
        {
            _currentUser = await _identityService.GetCurrentUserAsync(cancellationToken);

            return await GetByRiskIdQuery(riskId)
                .WhereAll((await GetRiskTrackingReportsExpressions(_currentUser, cancellationToken)).ToPredicates())
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }

        private async Task<DataExpressions<RiskTrackingReport>> GetRiskTrackingReportsExpressions(UserModel currentUser, CancellationToken cancellationToken)
        {
            var dataExpressions = new DataExpressions<RiskTrackingReport>();

            dataExpressions.AddRange(await GetRiskTrackingReportsUserRightsExpressionsAsync(currentUser, cancellationToken));

            return dataExpressions;
        }

        private Task<DataExpressions<RiskTrackingReport>> GetRiskTrackingReportsUserRightsExpressionsAsync(UserModel currentUser, CancellationToken cancellationToken)
        {
            var rightsComponent = new RiskTrackingReportsPermissionsFilterComponent(_serviceProvider);
            var rightsComponentContext = new RiskTrackingReportsPermissionsFilterComponentContext
            {
                CurrentUser = currentUser
            };

            return rightsComponent.ExtractDataExpressionsAsync(rightsComponentContext, cancellationToken);
        }

        public async Task<RiskTrackingReport> CreateRiskTrackingReportAsync(RiskTrackingReport riskTrackingReport, CancellationToken cancellationToken)
        {
            await _dbContext.RiskTrackingReports.AddAsync(riskTrackingReport, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return riskTrackingReport;
        }

        public IQueryable<RiskTrackingReport> GetByRiskIdQuery(long riskId)
        {
             return _dbContext.RiskTrackingReports
                              .Include(x => x.RiskActionProposals)
                              .Where(risk => risk.RiskId == riskId);
        }
    }
}
