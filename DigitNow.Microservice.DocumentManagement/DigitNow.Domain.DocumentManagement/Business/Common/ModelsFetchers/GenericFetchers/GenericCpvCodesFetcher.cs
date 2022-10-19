using AutoMapper;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers
{
    internal sealed class GenericCpvCodesFetcher : ModelFetcher<CpvCodeModel, ModelFetcherContext>
    {
        private readonly ICatalogClient _catalogClient;
        private readonly IMapper _mapper;

        public GenericCpvCodesFetcher(IServiceProvider serviceProvider)
        {
            _catalogClient = serviceProvider.GetService<ICatalogClient>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        protected override async Task<List<CpvCodeModel>> FetchInternalAsync(ModelFetcherContext context, CancellationToken cancellationToken)
        {
            var cpvCodesResponse = await _catalogClient.CpvCodesClient.GetCpvCodesAsync(cancellationToken);

            var cpvCodesModels = cpvCodesResponse.CpvCodes
                .Select(x => _mapper.Map<CpvCodeModel>(x))
                .ToList();

            return cpvCodesModels;
        }
    }
}
