using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Microservice.TenantNotification
{
    public static class IConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder ConfigureApi(this IConfigurationBuilder configurationBuilder,
            IHostEnvironment environment)
        {
            string appSettingsJson = "appsettings.json";
            if (environment.IsDevelopment())
                appSettingsJson = "appsettings.Development.json";

            configurationBuilder.AddJsonFile(appSettingsJson, true, true);

            return configurationBuilder;
        }
    }
}