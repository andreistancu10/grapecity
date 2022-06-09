using System;
using System.Reflection;
using HTSS.Platform.Infrastructure.ElasticsearchProvider;
using HTSS.Platform.Infrastructure.ElasticsearchProvider.MultiTenant.MicrosoftExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace DigitNow.Domain.DocumentManagement.configurations
{
    public static class ConfigureElasticContextExtensions
    {
        private const string _applicationConfigurationDbContextSection = "DocumentManagementConnectionStringQuery:ElasticSearch";

        private static readonly Assembly _currentAssembly = typeof(DomainServiceExtensions).Assembly;

        public static IServiceCollection AddElasticSearchClient(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection(_applicationConfigurationDbContextSection).Get<ElasticSearchOptions>();
            services.AddSingleton<IElasticClient>(serviceProvider =>
            {
                var elasticSearchUri = new Uri(options.Url);
                ConnectionSettings connectionSettings = new ConnectionSettings(elasticSearchUri).BasicAuthentication(options.Username, options.Password);
                connectionSettings.ApplyFromAssembly(_currentAssembly);
                return new ElasticClient(connectionSettings);
            });

            services.AddElasticSearchIndexNameServiceCollection(configuration);
            return services;
        }
    }

    public class ElasticSearchOptions
    {
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}