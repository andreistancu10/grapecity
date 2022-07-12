using System;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers
{
    internal interface IModelFetcherContext : IDictionary<string, object>
    { }

    internal abstract class ModelFetcherContext : Dictionary<string, object>, IModelFetcherContext
    {
        protected ModelFetcherContext() : base(StringComparer.OrdinalIgnoreCase) { }
    }
}
