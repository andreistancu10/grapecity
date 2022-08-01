using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.GetFiles
{
    public class GetFilesHandler : IQueryHandler<GetFilesQuery, List<FileViewModel>>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUploadedFileService _uploadedFileService;
        private readonly UploadedFileRelationsFetcher _uploadedFileRelationsFetcher;

        public GetFilesHandler(
            DocumentManagementDbContext dbContext,
            IMapper mapper,
            IServiceProvider serviceProvider,
            IUploadedFileService uploadedFileService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _uploadedFileService = uploadedFileService;
            _uploadedFileRelationsFetcher = new UploadedFileRelationsFetcher(serviceProvider);
        }

        public async Task<List<FileViewModel>> Handle(GetFilesQuery query, CancellationToken cancellationToken)
        {
            var uploadedFiles = await _uploadedFileService.FetchUploadedFiles(query.DocumentId, cancellationToken);

            await _uploadedFileRelationsFetcher
                .TriggerFetchersAsync(new UploadedFilesFetcherContext
                {
                    UploadFiles = uploadedFiles
                }, cancellationToken);

            return uploadedFiles.Select(file =>
                new VirtualFileAggregate
                {
                    UploadedFile = file,
                    Categories = _uploadedFileRelationsFetcher.UploadedFileCategoryModels,
                    Users = _uploadedFileRelationsFetcher.UploadedFileUsers
                })
                .Select(aggregate => _mapper.Map<VirtualFileAggregate, FileViewModel>(aggregate))
                .ToList();
        }
    }
}