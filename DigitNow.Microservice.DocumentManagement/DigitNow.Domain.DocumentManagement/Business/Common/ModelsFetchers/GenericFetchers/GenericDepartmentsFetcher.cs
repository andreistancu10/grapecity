using AutoMapper;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers
{
    internal sealed class GenericDepartmentsFetcher : ModelFetcher<DepartmentModel, ModelFetcherContext>        
    {
        private readonly ICatalogClient _catalogClient;
        private readonly IMapper _mapper;

        public GenericDepartmentsFetcher(IServiceProvider serviceProvider)
        {
            _catalogClient = serviceProvider.GetService<ICatalogClient>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        protected override async Task<List<DepartmentModel>> FetchInternalAsync(ModelFetcherContext context, CancellationToken cancellationToken)
        {
            var departmentsResponse = await _catalogClient.Departments.GetDepartmentsAsync(cancellationToken);
            
            var documentDepartmentModels = departmentsResponse.Departments
                .Select(x => _mapper.Map<DepartmentModel>(x))
                .ToList();

            return documentDepartmentModels;
        }
    }
}