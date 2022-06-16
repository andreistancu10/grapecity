using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DigitNow.Domain.DocumentManagement.configurations;

public static class IConfigurationBuilderExtensions
{
    public static IConfigurationBuilder ConfigureTenantNotificationDomain(
        this IConfigurationBuilder configurationBuilder,
        IHostEnvironment environment)
    {
        string appSettingsJson = "domain.DocumentManagementSettings.json";
        if (!environment.IsProduction())
            appSettingsJson = "domain.DocumentManagementSettings.Development.json";

        configurationBuilder.AddJsonFile(appSettingsJson, true, true);

        return configurationBuilder;
    }
}