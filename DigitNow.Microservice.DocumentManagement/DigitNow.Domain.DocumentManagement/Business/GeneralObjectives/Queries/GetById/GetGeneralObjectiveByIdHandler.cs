using AutoMapper;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;

namespace DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Queries.GetById
{
    public class GetGeneralObjectiveByIdHandler : IQueryHandler<GetGeneralObjectiveByIdQuery, GeneralObjectiveViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IGeneralObjectiveService _generalObjectiveService;
        private readonly GeneralObjectiveRelationsFetcher _generalObjectiveRelationsFetcher;
        private readonly ICatalogClient _catalogClient;

        public GetGeneralObjectiveByIdHandler(IMapper mapper,
            IGeneralObjectiveService generalObjectiveService,
            IServiceProvider serviceProvider,
            ICatalogClient catalogClient)
        {
            _mapper = mapper;
            _generalObjectiveService = generalObjectiveService;
            _catalogClient = catalogClient;
            _generalObjectiveRelationsFetcher = new GeneralObjectiveRelationsFetcher(serviceProvider);
        }

        public async Task<GeneralObjectiveViewModel> Handle(GetGeneralObjectiveByIdQuery request, CancellationToken cancellationToken)
        {
            var scimStates = await _catalogClient.ScimStates.GetScimStateCodeIdAsync("activ", cancellationToken);
            var generalObjective = await _generalObjectiveService.FindQuery()
                .Where(item => item.ObjectiveId == request.ObjectiveId)
                .Include(item => item.Objective)
                .FirstOrDefaultAsync(cancellationToken);

            await _generalObjectiveRelationsFetcher
                .UseGeneralObjectivesContext(new GeneralObjectivesFetcherContext { GeneralObjectives = new List<GeneralObjective> { generalObjective } })
                .TriggerFetchersAsync(cancellationToken);

            if (generalObjective == null)
            {
                return null;
            }

            var aggregate = new GeneralObjectiveAggregate
            {
                GeneralObjective = generalObjective,
                Users = _generalObjectiveRelationsFetcher.GeneralObjectiveUsers
            };

            return _mapper.Map<GeneralObjectiveViewModel>(aggregate);
        }
    }
}
