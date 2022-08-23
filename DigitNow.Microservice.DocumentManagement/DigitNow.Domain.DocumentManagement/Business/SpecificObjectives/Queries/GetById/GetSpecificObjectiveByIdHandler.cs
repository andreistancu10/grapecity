using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Queries.GetById
{
    public class GetSpecificObjectiveByIdHandler : IQueryHandler<GetSpecificObjectiveByIdQuery, SpecificObjectiveViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ISpecificObjectiveService _specificObjectiveService;
        private readonly IUploadedFileService _uploadedFileService;

        public GetSpecificObjectiveByIdHandler(IMapper mapper,
            ISpecificObjectiveService specificObjectiveService,
            IUploadedFileService uploadedFileService)
        {
            _mapper = mapper;
            _specificObjectiveService = specificObjectiveService;
            _uploadedFileService = uploadedFileService;
        }

        public async Task<SpecificObjectiveViewModel> Handle(GetSpecificObjectiveByIdQuery request, CancellationToken cancellationToken)
        {
            var specificObjective = await _specificObjectiveService.FindQuery()
                .Where(item => item.ObjectiveId == request.ObjectiveId)
                .Include(item => item.Objective)
                .Include(item => item.AssociatedGeneralObjective.Objective)
                .Include(item => item.SpecificObjectiveFunctionarys).FirstOrDefaultAsync(cancellationToken);

            if (specificObjective == null)
            {
                return null;
            }

            var files = await _uploadedFileService.GetUploadedFileMappingsAsync(
                new List<long>
                {
                    specificObjective.Id
                }, TargetEntity.Objective, cancellationToken);
            
            var aggregate = new SpecificObjectiveAggregate
            {
                SpecificObjective = specificObjective,
                DocumentFileMappingModels = files.Select(c => _mapper.Map<DocumentFileMappingModel>(c)).ToList()
            };

            return _mapper.Map<SpecificObjectiveViewModel>(aggregate);
        }
    }
}
