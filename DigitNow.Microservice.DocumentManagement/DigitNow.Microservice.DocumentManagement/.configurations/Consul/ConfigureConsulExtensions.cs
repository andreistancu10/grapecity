using System;
using System.Net.Http;
using HTSS.Platform.Infrastructure.Consul.MicrosoftExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

namespace DigitNow.Microservice.DocumentManagement.configurations.Consul
{
    public static class ConfigureConsulExtensions
    {
        public static IServiceCollection AddConsulConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            var consulOptions = configuration.GetSection("Consul").Get<ConsulOptions>();
            services.Configure<ConsulOptions>(options =>
            {
                options.AgentName = consulOptions.AgentName;
                options.Scheme = consulOptions.Scheme;
                options.Host = consulOptions.Host;
                options.Port = consulOptions.Port;
            });
            services.AddConsul(options => options.Address = new Uri($"{consulOptions.Scheme}://{consulOptions.Host}:{consulOptions.Port}"));
            return services;
        }

        public static IApplicationBuilder UseConsulConfigurations(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            IOptions<ConsulOptions> options = app.ApplicationServices.GetService<IOptions<ConsulOptions>>();
            ILogger<IApplicationBuilder> logger = app.ApplicationServices.GetService<ILogger<IApplicationBuilder>>();

            string discoveryAddress = Environment.GetEnvironmentVariable("DISCOVERY_ADDRESS");
            if (!Uri.IsWellFormedUriString(discoveryAddress, UriKind.Absolute))
            {
                logger.LogWarning("Consul: Microservice registration failed. Please make sure that EnvironmentVariable:DISCOVERY_ADDRESS is defined!");
                return app;
            }

            var address = new Uri(discoveryAddress);

            var asyncRetryPolicy = Policy.Handle<HttpRequestException>()
                .WaitAndRetryForeverAsync(
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, count, context) => logger.LogError($"Polly: Retry {count} consul registration"));

            app.RegisterWithConsul(asyncRetryPolicy, agent =>
            {
                agent.ID = $"HTSS-API-{Environment.MachineName}-{options.Value.AgentName}-{address.Host}-{address.Port}";
                logger.LogInformation($"Consul: MicroserviceId: {agent.ID}");

                agent.Name = options.Value.AgentName;
                logger.LogInformation($"Consul: MicroserviceName: {agent.Name}");

                agent.Address = address.Host;
                agent.Port = address.Port;
                logger.LogInformation("Consul: Registration succeeded!");
            });

            return app;
        }
    }
}