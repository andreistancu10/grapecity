using Microsoft.Extensions.Configuration;

namespace DigitNow.Domain.DocumentManagement.configurations;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder ConfigureTenantNotificationDomain(this IConfigurationBuilder configurationBuilder)
    {
        return configurationBuilder.AddJsonFile("domain.DocumentManagementSettings.json", true, true);
    }
}