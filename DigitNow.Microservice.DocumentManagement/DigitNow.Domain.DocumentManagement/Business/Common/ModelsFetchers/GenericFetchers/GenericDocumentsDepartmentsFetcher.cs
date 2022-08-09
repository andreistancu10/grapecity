using AutoMapper;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers
{
    internal sealed class GenericDocumentsDepartmentsFetcher : ModelFetcher<DocumentDepartmentModel, ModelFetcherContext>        
    {
        private readonly ICatalogClient _catalogClient;
        private readonly IMapper _mapper;

        public GenericDocumentsDepartmentsFetcher(IServiceProvider serviceProvider)
        {
            _catalogClient = serviceProvider.GetService<ICatalogClient>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        protected override async Task<List<DocumentDepartmentModel>> FetchInternalAsync(ModelFetcherContext context, CancellationToken cancellationToken)
        {
            var departmentsResponse = await _catalogClient.Departments.GetDepartmentsAsync(cancellationToken);

            var documentDepartmentModels = departmentsResponse.Departments
                .Select(x => _mapper.Map<DocumentDepartmentModel>(x))
                .ToList();

            return documentDepartmentModels;
        }
    }
}