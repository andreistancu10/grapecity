using AutoMapper;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers
{
    internal sealed class GenericEstablishedProceduresFetcher : ModelFetcher<EstablishedProcedureModel, ModelFetcherContext>
    {
        private readonly ICatalogClient _catalogClient;
        private readonly IMapper _mapper;

        public GenericEstablishedProceduresFetcher(IServiceProvider serviceProvider)
        {
            _catalogClient = serviceProvider.GetService<ICatalogClient>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        protected override async Task<List<EstablishedProcedureModel>> FetchInternalAsync(ModelFetcherContext context, CancellationToken cancellationToken)
        {
            var establishedProceduresResponse = await _catalogClient.EstablishedProceduresClient.GetEstablishedProceduresAsync(cancellationToken);

            var establishedProceduresModels = establishedProceduresResponse.EstablishedProcedures
                .Select(x => _mapper.Map<EstablishedProcedureModel>(x))
                .ToList();

            return establishedProceduresModels;
        }
    }
}
