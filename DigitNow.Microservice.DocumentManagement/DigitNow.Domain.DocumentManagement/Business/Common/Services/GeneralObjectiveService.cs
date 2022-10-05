using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.Scim;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IGeneralObjectiveService : IScimStateService
    {
        Task<GeneralObjective> AddAsync(GeneralObjective generalObjective, CancellationToken cancellationToken);
        Task UpdateAsync(GeneralObjective generalObjective, CancellationToken cancellationToken);
        IQueryable<GeneralObjective> FindQuery();
    }

    public class GeneralObjectiveService : ScimStateService, IGeneralObjectiveService
    {
        private readonly IObjectiveService _objectiveService;
        private readonly ICatalogClient _catalogClient;

        public GeneralObjectiveService(
            DocumentManagementDbContext dbContext,
            IObjectiveService objectiveService,
            ICatalogClient catalogClient) : base(dbContext)
        {
            _objectiveService = objectiveService;
            _catalogClient = catalogClient;
        }

        public async Task<GeneralObjective> AddAsync(GeneralObjective generalObjective, CancellationToken cancellationToken)
        {
            generalObjective.Objective ??= new Objective();

            generalObjective.Objective.ObjectiveType = ObjectiveType.General;
            var activeScimState = await _catalogClient.ScimStates.GetScimStateByCodeAsync("activ", cancellationToken);
            generalObjective.Objective.StateId = activeScimState.Id;

            await _objectiveService.AddAsync(generalObjective.Objective, cancellationToken);
            await DbContext.AddAsync(generalObjective, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);

            return generalObjective;
        }

        public IQueryable<GeneralObjective> FindQuery()
        {
            return DbContext.GeneralObjectives.AsQueryable();
        }

        public async Task UpdateAsync(GeneralObjective generalObjective, CancellationToken cancellationToken)
        {
            await ChangeStateAsync(new List<long> { generalObjective.ObjectiveId }, ScimEntity.GeneralObjective, generalObjective.Objective?.StateId, cancellationToken);
            await DbContext.SingleUpdateAsync(generalObjective, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}