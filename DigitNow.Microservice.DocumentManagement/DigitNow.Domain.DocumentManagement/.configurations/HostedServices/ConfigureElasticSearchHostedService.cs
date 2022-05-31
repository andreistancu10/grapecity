using System;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Notifications;
using DigitNow.Domain.DocumentManagement.Data.NotificationTypeCoverGapExtensions;
using HTSS.Platform.Infrastructure.MultiTenant;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;

namespace DigitNow.Domain.DocumentManagement.configurations.HostedServices
{
    public class ConfigureElasticSearchHostedService : IHostedService
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<ConfigureElasticSearchHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ConfigureElasticSearchHostedService(
            IElasticClient elasticClient,
            ILogger<ConfigureElasticSearchHostedService> logger,
            IServiceProvider serviceProvider)
        {
            _elasticClient = elasticClient;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // check for multi tenant setting
            IConfiguration configuration = _serviceProvider.GetRequiredService<IConfiguration>();

            try
            {
                if (configuration.GetValue<bool>(MultiTenantOptions.EnableMultiTenant))
                {
                    TenantInfoLoader tenantInfoLoader = _serviceProvider.GetRequiredService<TenantInfoLoader>();

                    var tenants = tenantInfoLoader.GetTenants();

                    foreach (var tenantInfo in tenants)
                    {
                        // create/update indexes for each tenant
                        await CreateUpdateIndexMultiTenant<NotificationElastic>(IndexConfiguration.Notification, tenantInfo, cancellationToken);
                        await CreateUpdateIndexMultiTenant<NotificationTypeCoverGapExtensionElastic>(IndexConfiguration.NotificationTypeCoverGapExtension, tenantInfo, cancellationToken);
                    }
                }
                else
                {
                    // create/update indexes for single tenant
                    await CreateUpdateIndex<NotificationElastic>(IndexConfiguration.Notification, cancellationToken);
                    await CreateUpdateIndex<NotificationTypeCoverGapExtensionElastic>(IndexConfiguration.NotificationTypeCoverGapExtension, cancellationToken);
                }

                _logger.LogInformation("Elasticsearch configuration applied");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on applying elasticsearch configuration");
                throw;
            }
        }

        private async Task CreateUpdateIndexMultiTenant<T>(string indexNameFromEnumConfig, TenantInfo tenantInfo, CancellationToken cancellationToken) where T : class
        {
            var indexName = IndexNameMultiTenant(indexNameFromEnumConfig, tenantInfo);
            var status = await _elasticClient.Indices.ExistsAsync(indexName, null, cancellationToken);
            if (!status.Exists)
            {
                //create a new index and apply mapping for fields
                await _elasticClient.Indices.CreateAsync(indexName,
                    index => index.Map<T>(x => x.AutoMap()), cancellationToken);
            }
            else
            {
                //only apply mapping for fields
                await _elasticClient.MapAsync<T>(s => s.Index(indexName).AutoMap(), cancellationToken);
            }
        }

        private string IndexNameMultiTenant(string indexNameFromEnumConfig, TenantInfo tenantInfo)
        {
            var indexName = indexNameFromEnumConfig + "-" + tenantInfo.TenantCode.ToLower() + "-" + tenantInfo.TenantId;
            return indexName.ToLower();
        }

        private async Task CreateUpdateIndex<T>(string indexNameFromEnumConfig, CancellationToken cancellationToken) where T : class
        {
            var status = await _elasticClient.Indices.ExistsAsync(indexNameFromEnumConfig, null, cancellationToken);
            if (!status.Exists)
            {
                //create a new index and apply mapping for fields
                await _elasticClient.Indices.CreateAsync(indexNameFromEnumConfig,
                    index => index.Map<T>(x => x.AutoMap()), cancellationToken);
            }
            else
            {
                //only apply mapping for fields
                await _elasticClient.MapAsync<T>(s => s.Index(indexNameFromEnumConfig).AutoMap(), cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}