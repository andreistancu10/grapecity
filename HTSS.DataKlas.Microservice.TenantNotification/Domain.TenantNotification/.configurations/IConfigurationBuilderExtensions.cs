using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ShiftIn.Domain.TenantNotification
{
    public static class IConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder ConfigureTenantNotificationDomain(
            this IConfigurationBuilder configurationBuilder,
            IHostEnvironment environment)
        {
            string appSettingsJson = "domain.TenantNotificationSettings.json";
            if (!environment.IsProduction())
                appSettingsJson = "domain.TenantNotificationSettings.Development.json";

            configurationBuilder.AddJsonFile(appSettingsJson, true, true);

            return configurationBuilder;
        }
    }
}