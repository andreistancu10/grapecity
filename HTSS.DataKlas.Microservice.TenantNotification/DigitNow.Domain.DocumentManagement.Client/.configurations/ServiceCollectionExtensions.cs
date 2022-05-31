using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;
using ShiftIn.Domain.TenantNotification.Client.Internal;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications.ChangeNotificationsStatusForEntity;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications.ChangeNotificationStatus;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications.CreateNotification;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications.GetNotificationById;
using ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions.GetList;

namespace ShiftIn.Domain.TenantNotification.Client.configurations
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