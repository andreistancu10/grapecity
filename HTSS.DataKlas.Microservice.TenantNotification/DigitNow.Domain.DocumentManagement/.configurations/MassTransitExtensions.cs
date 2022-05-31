using Domain.Localization.Client.configurations;
using GreenPipes;
using HTSS.Platform.Infrastructure.MassTransit.MultiTenant;
using HTSS.Platform.Infrastructure.MultiTenant;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;
using ShiftIn.Domain.Authentication.Client;
using ShiftIn.Domain.TenantNotification.Business.Consumers;
using ShiftIn.Domain.TenantNotification.Business.Notifications.Consumers.GetNotificationById;
using ShiftIn.Domain.TenantNotification.Business.Notifications.Consumers.UpdateElastic;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Consumers;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Consumers.GetList;
using System;

namespace ShiftIn.Domain.TenantNotification
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