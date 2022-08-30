using System;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts
{
    internal class LanguageFetcherContext : ModelFetcherContext
    {
        public int LanguageId
        {
            get => (int)this[nameof(LanguageId)];
            set => this[nameof(LanguageId)] = value;
        }
    }
}
