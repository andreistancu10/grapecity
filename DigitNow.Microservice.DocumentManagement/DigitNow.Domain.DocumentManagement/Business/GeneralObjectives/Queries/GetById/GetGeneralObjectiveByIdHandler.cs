using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;

namespace DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Queries.GetById
{
    public class GetGeneralObjectiveByIdHandler : IQueryHandler<GetGeneralObjectiveByIdQuery, GeneralObjectiveViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IGeneralObjectiveService _generalObjectiveService;
        private readonly IUploadedFileService _uploadedFileService;
        private readonly GeneralObjectiveRelationsFetcher _generalObjectiveRelationsFetcher;


        public GetGeneralObjectiveByIdHandler(IMapper mapper,
            IGeneralObjectiveService generalObjectiveService,
            IUploadedFileService uploadedFileService,
            IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _generalObjectiveService = generalObjectiveService;
            _uploadedFileService = uploadedFileService;
            _generalObjectiveRelationsFetcher = new GeneralObjectiveRelationsFetcher(serviceProvider);
        }

        public async Task<GeneralObjectiveViewModel> Handle(GetGeneralObjectiveByIdQuery request, CancellationToken cancellationToken)
        {
            var generalObjective = await _generalObjectiveService.FindQuery()
                .Where(item => item.ObjectiveId == request.ObjectiveId)
                .Include(item => item.Objective)
                .FirstOrDefaultAsync(cancellationToken);

            await _generalObjectiveRelationsFetcher
                .UseGeneralObjectivesContext(new GeneralObjectivesFetcherContext { GeneralObjectives = new List<GeneralObjective>() { generalObjective } })
                .TriggerFetchersAsync(cancellationToken);

            if (generalObjective == null)
            {
                return null;
            }

            var aggregate = new GeneralObjectiveAggregate()
            {
                GeneralObjective = generalObjective,
                Users = _generalObjectiveRelationsFetcher.GeneralObjectiveUsers
            };

            return _mapper.Map<GeneralObjectiveViewModel>(aggregate);
        }
    }
}
