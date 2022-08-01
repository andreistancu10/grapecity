using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Adapters.MS.Catalog;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal sealed class DocumentsDepartmentsFetcher : ModelFetcher<DocumentDepartmentModel, DocumentsFetcherContext>
    {
        private readonly ICatalogClient _catalogClient;
        private readonly ICatalogAdapterClient _catalogAdapterClient;
        private readonly IMapper _mapper;

        public DocumentsDepartmentsFetcher(IServiceProvider serviceProvider)
        {
            _catalogClient = serviceProvider.GetService<ICatalogClient>();
            _catalogAdapterClient = serviceProvider.GetService<ICatalogAdapterClient>();
            _mapper = serviceProvider.GetService<IMapper>();
        }


        protected override async Task<List<DocumentDepartmentModel>> FetchInternalAsync(DocumentsFetcherContext context, CancellationToken cancellationToken)
        {
            var departments = await _catalogAdapterClient.GetDepartmentsAsync(cancellationToken);

            var documentDepartmentModels = departments
                .Select(x => _mapper.Map<DocumentDepartmentModel>(x))
                .ToList();

            return documentDepartmentModels;
        }

        [Obsolete("This will be investigated in the future")]
        private async Task<List<DocumentDepartmentModel>> FetchInternalAsync_Rpc(DocumentsFetcherContext context, CancellationToken cancellationToken)
        {
            var departmentsResponse = await _catalogClient.Departments.GetDepartmentsAsync(cancellationToken);

            var documentDepartmentModels = departmentsResponse.Departments
                .Select(x => _mapper.Map<DocumentDepartmentModel>(x))
                .ToList();

            return documentDepartmentModels;
        }
    }
}