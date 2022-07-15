using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal sealed class DocumentsCategoriesFetcher : ModelFetcher<DocumentCategoryModel, DocumentsFetcherContext>
    {
        private readonly ICatalogClient _catalogClient;
        private readonly IMapper _mapper;

        public DocumentsCategoriesFetcher(IServiceProvider serviceProvider)
        {
            _catalogClient = serviceProvider.GetService<ICatalogClient>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        public override async Task<IReadOnlyList<DocumentCategoryModel>> FetchAsync(DocumentsFetcherContext context, CancellationToken cancellationToken)
        {
            var documentTypesResponse = await _catalogClient.DocumentTypes.GetDocumentTypesAsync(cancellationToken);

            // Note: DocumentTypes is actual DocumentCategory
            var documentCategoryModels = documentTypesResponse.DocumentTypes
                .Select(x => _mapper.Map<DocumentCategoryModel>(x))
                .ToList();

            return documentCategoryModels;
        }
    }

    internal sealed class DocumentsDepartmentsFetcher : ModelFetcher<DocumentDepartmentModel, DocumentsFetcherContext>
    {
        private readonly ICatalogClient _catalogClient;
        private readonly IMapper _mapper;

        public DocumentsDepartmentsFetcher(IServiceProvider serviceProvider)
        {
            _catalogClient = serviceProvider.GetService<ICatalogClient>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        public override async Task<IReadOnlyList<DocumentDepartmentModel>> FetchAsync(DocumentsFetcherContext context, CancellationToken cancellationToken)
        {
            var departmentsResponse = await _catalogClient.Departments.GetDepartmentsAsync(cancellationToken);

            var documentDepartmentModels = departmentsResponse.Departments
                .Select(x => _mapper.Map<DocumentDepartmentModel>(x))
                .ToList();

            return documentDepartmentModels;
        }
    }

    internal sealed class DocumentsSpecialRegisterMappingFetcher : ModelFetcher<DocumentsSpecialRegisterMappingModel, DocumentsFetcherContext>
    {
        private readonly IMapper _mapper;
        private readonly DocumentManagementDbContext _dbContext;

        public DocumentsSpecialRegisterMappingFetcher(IServiceProvider serviceProvider)
        {
            _mapper = serviceProvider.GetService<IMapper>();
            _dbContext = serviceProvider.GetService<DocumentManagementDbContext>();
        }

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
}
