using DigitNow.Adapters.MS.Catalog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace DigitNow.Domain.DocumentManagement.configurations.Adapters
{
    public static class AdaptersExtensions
    {
        public static IServiceCollection AddAdapters(this IServiceCollection services, IConfiguration configuration)
        {
            var identityApi = GetConfigurationKey(configuration, "Adapters:Identity");
            var catalogApi = GetConfigurationKey(configuration, "Adapters:Catalog");

            services.AddTransient(serviceProvider => new CatalogHttpClient(serviceProvider, catalogApi));

            services.AddTransient<ICatalogAdapterClient, CatalogAdapterClient>();

            return services;
        }

        private static string GetConfigurationKey(IConfiguration configuration, string key)
        {
            var value = configuration.GetValue<string>(key);

            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                throw new ConfigurationErrorsException($"Invalid configuration! Key '{key}' is null or empty.");
            }

            return value;
        }

    }
}
