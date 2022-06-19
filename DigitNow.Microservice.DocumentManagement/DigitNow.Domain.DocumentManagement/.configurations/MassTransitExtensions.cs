using System;
using Domain.Localization.Client.configurations;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;
using ShiftIn.Domain.Authentication.Client;

namespace DigitNow.Domain.DocumentManagement.configurations;

public static class MassTransitExtensions
{
    public static void AddTenantNotificationMassTransitServiceConfigurations(this IServiceCollectionBusConfigurator serviceCollection)
    {
        serviceCollection.AddAuthenticationClientMassTransitServiceConfigurations();
        serviceCollection.AddLocalizationMQServicesConfigs();
    }

    public static void AddTenantNotificationMassTransitRabbitConfigurations(this IRabbitMqBusFactoryConfigurator rabbit, IBusRegistrationContext context, Action<IReceiveEndpointConfigurator> registerAction)
    {
        rabbit.ReceiveEndpoint(typeof(MassTransitExtensions).Assembly.GetName().Name, registerAction);
    }
}