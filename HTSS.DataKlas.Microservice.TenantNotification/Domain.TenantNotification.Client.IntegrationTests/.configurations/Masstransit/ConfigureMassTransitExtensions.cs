﻿using System.Reflection;
using GreenPipes;
using HTSS.Platform.Infrastructure.MassTransit;
using HTSS.Platform.Infrastructure.MassTransit.MicrosoftExtensions;
using HTSS.Platform.Infrastructure.MassTransit.MultiTenant;
using HTSS.Platform.Infrastructure.MultiTenant;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftIn.Domain.TenantNotification.Client;
using ShiftIn.Domain.TenantNotification.Client.configurations;

namespace ShiftIn.Domain.TenantNotification.Client.IntegrationTests
{
    public static class ConfigureMassTransitExtensions
    {
        private const string MassTransitSection = "MassTransit";
        private static string CurrentMicroservice => typeof(ConfigureMassTransitExtensions).Assembly.GetName().Name;
        private static Assembly IntegrationsTestsDomain => typeof(ConfigureMassTransitExtensions).Assembly;

        public static IServiceCollection AddMassTransitConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            var massTransitOptions = configuration.GetSection(MassTransitSection).Get<MassTransitOptions>();

            void AddOptions(MassTransitOptions options)
            {
                options.Microservice = CurrentMicroservice;
                options.Host = massTransitOptions.Host;
                options.Password = massTransitOptions.Password;
                options.Port = massTransitOptions.Port;
                options.User = massTransitOptions.User;
                options.VirtualHost = massTransitOptions.VirtualHost;
            }

            void AddServiceConfigurations(IServiceCollectionBusConfigurator serviceCollection)
            {
                serviceCollection.AddTenantNotificationMQServicesConfigs();
            }

            void AddFactoryConfigurations(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator rabbit)
            {
                rabbit.Host(massTransitOptions.Host, (ushort)massTransitOptions.Port, massTransitOptions.VirtualHost,
                    _ =>
                    {
                        _.Username(massTransitOptions.User);
                        _.Password(massTransitOptions.Password);
                    });

                var enableMultiTenant = configuration.GetValue<bool>(MultiTenantOptions.EnableMultiTenant);
                if (enableMultiTenant)
                {
                    rabbit.UseMessageScope(context);

                    rabbit.UseInlineFilter((consumeContext, pipe) =>
                    {
                        if (!consumeContext.TryGetPayload<IServiceScope>(out var scope)) return pipe.Send(consumeContext);
                        var tenantService = scope.ServiceProvider.GetRequiredService<ITenantService>();

                        var tenantId = consumeContext.Headers.Get<long>(MassTransitMultiTenantConstants.MultiTenantHeaderName);

                        if (tenantId is > 0) tenantService.SetTenantInfo(tenantId.Value);

                        return pipe.Send(consumeContext);
                    });
                    rabbit.UseSendFilter(typeof(TenantSendFilter<>), context);
                    rabbit.UsePublishFilter(typeof(TenantPublishFilter<>), context);
                }
            }

            return services.AddMassTransitServices<ConfigureApiBusHostedService>(AddOptions, AddServiceConfigurations,
                AddFactoryConfigurations, new[]
                {
                    IntegrationsTestsDomain
                });
        }
    }
}
