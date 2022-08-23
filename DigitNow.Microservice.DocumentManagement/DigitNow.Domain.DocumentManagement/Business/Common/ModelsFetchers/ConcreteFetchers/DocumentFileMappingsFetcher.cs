using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class DocumentFileMappingsFetcher : ModelFetcher<DocumentFileMappingModel, UploadedFilesFetcherContext>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public DocumentFileMappingsFetcher(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<DocumentManagementDbContext>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        protected override async Task<List<DocumentFileMappingModel>> FetchInternalAsync(UploadedFilesFetcherContext context, CancellationToken cancellationToken)
        {
            var uploadedFileMappingIds = context.UploadedFiles.Select(c => c.UploadedFileMappingId);
            var documentFileMappings = await _dbContext.DocumentFileMappings
                .Where(c => uploadedFileMappingIds.Contains(c.UploadedFileMappingId))
                .ToListAsync(cancellationToken);

            return documentFileMappings.Select(c => _mapper.Map<DocumentFileMappingModel>(c)).ToList();
        }
    }
}