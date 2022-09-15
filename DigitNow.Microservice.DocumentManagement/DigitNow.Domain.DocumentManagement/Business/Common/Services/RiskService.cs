using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.Risks.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Risks;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IRiskService : IScimStateService
    {
        Task<Risk> AddAsync(Risk Risk, CancellationToken cancellationToken);
        Task UpdateAsync(Risk Risk, CancellationToken cancellationToken);
        IQueryable<Risk> FindQuery();
    }

    public class RiskService : ScimStateService, IRiskService
    {
        public RiskService(DocumentManagementDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Risk> AddAsync(Risk risk, CancellationToken token)
        {
            risk.State = ScimState.Active;
            risk.RiskExposureEvaluation =
                CalculateRiskExposureEvaluation(risk.ProbabilityOfApparitionEstimation, risk.ImpactOfObjectivesEstimation);
            var dbContextTransaction = await DbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, token);
            try
            {
                DbContext.Entry(risk).State = EntityState.Added;
                await SetRiskCodeAsync(risk, token);
                await DbContext.SaveChangesAsync(token);
                await dbContextTransaction.CommitAsync(token);
            }
            catch
            {
                await dbContextTransaction.RollbackAsync(token);
                throw;
            }
            finally
            {
                dbContextTransaction?.Dispose();
            }

            return risk;
        }

        public IQueryable<Risk> FindQuery()
        {
            return DbContext.Risks.AsQueryable();
        }

        public async Task UpdateAsync(Risk Risk, CancellationToken cancellationToken)
        {
            await ChangeStateAsync(new List<long> { Risk.Id }, ScimEntity.ScimRisk, Risk.State, cancellationToken);
            await DbContext.SingleUpdateAsync(Risk, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task SetRiskCodeAsync(Risk risk, CancellationToken token)
        {
            var lastRisk = await DbContext.Risks
                .Where(item => item.CreatedAt.Year == DateTime.UtcNow.Year)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync(token);

            var order = lastRisk != null ? lastRisk.Id : 0;

            risk.Code = $"RISK{DateTime.Now.Year}_{++order}";
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
    }
}
