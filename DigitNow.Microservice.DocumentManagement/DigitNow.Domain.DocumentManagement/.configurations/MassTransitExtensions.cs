using System;
using DigitNow.Domain.Catalog.Client.configurations;
using Domain.Authentication.Client.configurations;
using Domain.Localization.Client.configurations;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;
using ShiftIn.Domain.Authentication.Client;

namespace DigitNow.Domain.DocumentManagement.configurations;

public static class MassTransitExtensions
{
    public static void AddDocumentManagementMassTransitServiceConfigurations(this IServiceCollectionBusConfigurator serviceCollection)
    {
        serviceCollection.AddAuthenticationClientMassTransitServiceConfigurations();
        serviceCollection.AddLocalizationMQServicesConfigs();
        serviceCollection.AddCatalogClientMassTransitServiceConfigurations();
        serviceCollection.AddIdentityMQServicesConfigs();
    }

    public static void AddDocumentManagementMassTransitRabbitConfigurations(this IRabbitMqBusFactoryConfigurator rabbit, IBusRegistrationContext context, Action<IReceiveEndpointConfigurator> registerAction)
    {
        rabbit.ReceiveEndpoint(typeof(MassTransitExtensions).Assembly.GetName().Name, registerAction);
    }
}