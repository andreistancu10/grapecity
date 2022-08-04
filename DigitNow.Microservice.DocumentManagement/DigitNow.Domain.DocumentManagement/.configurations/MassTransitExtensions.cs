using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.Catalog.Client.configurations;
using Domain.Localization.Client.configurations;
using Domain.Mail.Client;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;

namespace DigitNow.Domain.DocumentManagement.configurations;

public static class MassTransitExtensions
{
    public static void AddDocumentManagementMassTransitServiceConfigurations(this IServiceCollectionBusConfigurator serviceCollection)
    {
        serviceCollection.AddAuthenticationClientMassTransitServiceConfigurations();
        serviceCollection.AddLocalizationMQServicesConfigs();
        serviceCollection.AddCatalogClientMassTransitServiceConfigurations();
        serviceCollection.AddMailClientMassTransitServiceConfigurations();
    }

    public static void AddDocumentManagementMassTransitRabbitConfigurations(this IRabbitMqBusFactoryConfigurator rabbit, IBusRegistrationContext context, Action<IReceiveEndpointConfigurator> registerAction)
    {
        rabbit.ReceiveEndpoint(typeof(MassTransitExtensions).Assembly.GetName().Name, registerAction);
    }
}