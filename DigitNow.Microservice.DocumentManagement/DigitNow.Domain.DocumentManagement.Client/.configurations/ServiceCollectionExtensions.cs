using DigitNow.Domain.DocumentManagement.Client.Internal;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications.ChangeNotificationsStatusForEntity;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications.ChangeNotificationStatus;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications.CreateNotification;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications.GetNotificationById;
using DigitNow.Domain.DocumentManagement.Contracts.NotificationTypeCoverGapExtensions.GetList;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Client.configurations
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Register the MassTransit Request clients
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollectionBusConfigurator AddTenantNotificationMQServicesConfigs(this IServiceCollectionBusConfigurator services)
        {
            services.AddRequestClient<IGetNotificationByIdRequest>();
            services.AddRequestClient<ICreateNotificationEvent>();
            services.AddRequestClient<IChangeNotificationStatusEvent>();
            services.AddRequestClient<IChangeNotificationsStatusForEntityEvent>();
            services.AddRequestClient<IGetNotificationTypeCoverGapExtensionsRequest>();

            return services;
        }

        /// <summary>
        ///     Register the IdentityManager service in ServiceCollection. The method implements MassTransit RPC communication
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddTenantNotificationMQServices(this IServiceCollection services)
        {
            services.AddScoped<ITenantNotificationManager, TenantNotificationManager>();

            return services;
        }
    }
}