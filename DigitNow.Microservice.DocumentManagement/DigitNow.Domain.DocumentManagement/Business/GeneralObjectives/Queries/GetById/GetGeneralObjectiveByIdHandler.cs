using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;

namespace DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Queries.GetById
{
    public class GetGeneralObjectiveByIdHandler : IQueryHandler<GetGeneralObjectiveByIdQuery, GeneralObjectiveViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IGeneralObjectiveService _generalObjectiveService;
        private readonly IUploadedFileService _uploadedFileService;

        public GetGeneralObjectiveByIdHandler(IMapper mapper,
            IGeneralObjectiveService generalObjectiveService,
            IUploadedFileService uploadedFileService)
        {
            _mapper = mapper;
            _generalObjectiveService = generalObjectiveService;
            _uploadedFileService = uploadedFileService;
        }

        public async Task<GeneralObjectiveViewModel> Handle(GetGeneralObjectiveByIdQuery request, CancellationToken cancellationToken)
        {
            var generalObjective = await _generalObjectiveService.FindQuery()
                .Where(item => item.ObjectiveId == request.ObjectiveId)
                .Include(item => item.Objective)
                .FirstOrDefaultAsync(cancellationToken);

            if (generalObjective == null)
            {
                return null;
            }

            var files = await _uploadedFileService.GetUploadedFileMappingsAsync(
                new List<long>
                {
                    generalObjective.Id
                }, TargetEntity.Objective, cancellationToken);

            var aggregate = new GeneralObjectiveAggregate()
            {
                GeneralObjective = generalObjective,
                DocumentFileMappingModels = files.Select(c => _mapper.Map<DocumentFileMappingModel>(c)).ToList()
            };

            return _mapper.Map<GeneralObjectiveViewModel>(aggregate);
        }
    }
}
