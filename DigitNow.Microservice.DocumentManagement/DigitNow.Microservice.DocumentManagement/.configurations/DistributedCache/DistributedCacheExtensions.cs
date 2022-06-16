using HTSS.Platform.Infrastructure.Cache;
using HTSS.Platform.Infrastructure.Cache.MultiTenant;
using HTSS.Platform.Infrastructure.MultiTenant;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Caching.Cosmos;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Microservice.DocumentManagement.configurations.DistributedCache
{
    public static class DistributedCacheExtensions
    {
        private const string DistributedCache = "DistributedCache";

        public static IServiceCollection AddDistributedCacheConfigurations(this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = configuration.GetSection(DistributedCache).Get<DistributedCacheOptions>();

            void SetupRedis(RedisCacheOptions cacheOptions)
            {
                cacheOptions.Configuration = options.Configuration;
                cacheOptions.InstanceName = options.InstanceName;
            }

            void SetupInMemory(MemoryDistributedCacheOptions cacheOptions)
            {
            }

            void SetupCosmos(CosmosCacheOptions cacheOptions)
            {
                cacheOptions.CreateIfNotExists = true;
                cacheOptions.ContainerName = options.ContainerName;
                cacheOptions.DatabaseName = options.DatabaseName;
                cacheOptions.ClientBuilder = new CosmosClientBuilder(options.Configuration);
            }

            bool multiTenantEnabled = configuration.GetValue<bool>(MultiTenantOptions.EnableMultiTenant);

            if (multiTenantEnabled)
            {
                services.AddMultiTenantDistributedCache(options.Provider, SetupRedis, SetupInMemory, SetupCosmos);
            }
            else
            {
                services.AddDistributedCache(options.Provider, SetupRedis, SetupInMemory, SetupCosmos);
            }

            return services;
        }
    }
}