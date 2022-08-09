using System;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers
{
    internal interface IModelFetcherContext : IDictionary<string, object>
    { }

    public class ModelFetcherContext : Dictionary<string, object>, IModelFetcherContext
    {
        public ModelFetcherContext() : base(StringComparer.OrdinalIgnoreCase) { }
    }
}
