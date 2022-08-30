using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.utils;
using Domain.Localization.Client;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class DocumentTypeTranslationsFetcher : ModelFetcher<DocumentTypeTranslationModel, LanguageFetcherContext>
    {
        private readonly ILocalizationManager _localizationManager;

        public DocumentTypeTranslationsFetcher(IServiceProvider serviceProvider) 
        {
            _localizationManager = serviceProvider.GetService<ILocalizationManager>();
        }

        protected async override Task<List<DocumentTypeTranslationModel>> FetchInternalAsync(LanguageFetcherContext context, CancellationToken cancellationToken)
        {
            var translationResponse = await _localizationManager.Translate(
                context.LanguageId,
                CustomMappings.DocumentTypeTranslations.Select(c => c.Value),
                cancellationToken);

            var result = new List<DocumentTypeTranslationModel>();
            foreach (var storedTranslationPair in CustomMappings.DocumentTypeTranslations)
            {
                if (!translationResponse.Translations.ContainsKey(storedTranslationPair.Value)) continue;

                var foundTranslation = translationResponse.Translations.First(x => x.Key == storedTranslationPair.Value);

                if (!string.IsNullOrEmpty(foundTranslation.Value))
                {
                    result.Add(new DocumentTypeTranslationModel { DocumentType = storedTranslationPair.Key, Translation = foundTranslation.Value });
                }
            }
            return result;
        }
    }
}
