using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.utils;
using Domain.Localization.Client;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class DocumentStatusTranslationsFetcher : ModelFetcher<DocumentStatusTranslationModel, LanguageFetcherContext>
    {
        private readonly ILocalizationManager _localizationManager;

        public DocumentStatusTranslationsFetcher(IServiceProvider serviceProvider) 
        {
            _localizationManager = serviceProvider.GetService<ILocalizationManager>();
        }

        protected async override Task<List<DocumentStatusTranslationModel>> FetchInternalAsync(LanguageFetcherContext context, CancellationToken cancellationToken)
        {
            var translationResponse = await _localizationManager.Translate(
                context.LanguageId,
                CustomMappings.DocumentStatusTranslations.Select(c => c.Value),
                cancellationToken);

            var result = new List<DocumentStatusTranslationModel>();
            foreach (var storedTranslationPair in CustomMappings.DocumentStatusTranslations)
            {
                if (!translationResponse.Translations.ContainsKey(storedTranslationPair.Value)) continue;

                var foundTranslation = translationResponse.Translations.First(x => x.Key == storedTranslationPair.Value);

                if (!string.IsNullOrEmpty(foundTranslation.Value))
                {
                    result.Add(new DocumentStatusTranslationModel { Status = storedTranslationPair.Key, Translation = foundTranslation.Value });
                }
            }
            return result;
        }
    }
}
