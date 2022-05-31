using System;
using DigitNow.Domain.DocumentManagement.Business.Consumers;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Consumers.GetNotificationById;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Consumers.UpdateElastic;
using DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Consumers;
using DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Consumers.GetList;
using Domain.Localization.Client.configurations;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;
using ShiftIn.Domain.Authentication.Client;

namespace DigitNow.Domain.DocumentManagement.configurations
{
    public static class MassTransitExtensions
    {
        public static void AddTenantNotificationMassTransitServiceConfigurations(this IServiceCollectionBusConfigurator serviceCollection)
        {
            serviceCollection.AddConsumer<NotificationElasticUpdateIndexCommandConsumer>();
            serviceCollection.AddConsumer<GetNotificationByIdConsumer>();
            serviceCollection.AddConsumer<CreateNotificationEventConsumer>();
            serviceCollection.AddConsumer<ChangeNotificationStatusEventConsumer>();
            serviceCollection.AddConsumer<ChangeNotificationsStatusForEntityConsumer>();
            serviceCollection.AddConsumer<NotificationTypeCoverGapExtensionElasticUpdateIndexCommandConsumer>();
            serviceCollection.AddConsumer<GetNotificationTypeCoverGapExtensionsRequestResponseConsumer>();

            serviceCollection.AddAuthenticationClientMassTransitServiceConfigurations();
            serviceCollection.AddLocalizationMQServicesConfigs();
        }

        public static void AddTenantNotificationMassTransitRabbitConfigurations(this IRabbitMqBusFactoryConfigurator rabbit, IBusRegistrationContext context, Action<IReceiveEndpointConfigurator> registerAction)
        {
            rabbit.ReceiveEndpoint(typeof(MassTransitExtensions).Assembly.GetName().Name, registerConfig =>
            {
                registerAction(registerConfig);

                registerConfig.ConfigureConsumer<NotificationElasticUpdateIndexCommandConsumer>(context);
                registerConfig.ConfigureConsumer<GetNotificationByIdConsumer>(context);
                registerConfig.ConfigureConsumer<CreateNotificationEventConsumer>(context);
                registerConfig.ConfigureConsumer<ChangeNotificationStatusEventConsumer>(context);
                registerConfig.ConfigureConsumer<ChangeNotificationsStatusForEntityConsumer>(context);
                registerConfig.ConfigureConsumer<NotificationTypeCoverGapExtensionElasticUpdateIndexCommandConsumer>(context);
                registerConfig.ConfigureConsumer<GetNotificationTypeCoverGapExtensionsRequestResponseConsumer>(context);
            });
        }
    }
}