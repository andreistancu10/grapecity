using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;

internal sealed class DocumentsSpecialRegisterMappingFetcher : ModelFetcher<DocumentsSpecialRegisterMappingModel, DocumentsFetcherContext>
{
    private readonly IMapper _mapper;
    private readonly DocumentManagementDbContext _dbContext;

    public DocumentsSpecialRegisterMappingFetcher(IServiceProvider serviceProvider)
    {
        _mapper = serviceProvider.GetService<IMapper>();
        _dbContext = serviceProvider.GetService<DocumentManagementDbContext>();
    }

    public override bool IsInternal => true;

    public override async Task<IReadOnlyList<DocumentsSpecialRegisterMappingModel>> FetchAsync(DocumentsFetcherContext context, CancellationToken cancellationToken)
    {
        var documentIds = context.Documents.Select(c => c.Id);
        var documentsSpecialRegisterMappingModels =
            await _dbContext.SpecialRegisterMappings
                .AsNoTracking()
                .Where(c => documentIds.Contains(c.DocumentId))
                .Include(c => c.SpecialRegister)
                .Select(c => _mapper.Map<DocumentsSpecialRegisterMappingModel>(c))
                .ToListAsync(cancellationToken);

        return documentsSpecialRegisterMappingModels;
    }
}