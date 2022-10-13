using AutoMapper;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers
{
    internal sealed class GenericStatesFetcher : ModelFetcher<StateModel, ModelFetcherContext>
    {
        private readonly ICatalogClient _catalogClient;
        private readonly IMapper _mapper;

        public GenericStatesFetcher(IServiceProvider serviceProvider)
        {
            _catalogClient = serviceProvider.GetService<ICatalogClient>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        protected override async Task<List<StateModel>> FetchInternalAsync(ModelFetcherContext context, CancellationToken cancellationToken)
        {
            var scimStatesResponse = await _catalogClient.ScimStates.GetScimStatesAsync(cancellationToken);
            
            var scimStatesModels = scimStatesResponse.ScimStates
                .Select(x => _mapper.Map<StateModel>(x))
                .ToList();

            return scimStatesModels;
        }
    }
}