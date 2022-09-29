using Microsoft.Extensions.Configuration;

namespace DigitNow.Microservice.DocumentManagement.configurations;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder ConfigureApi(this IConfigurationBuilder configurationBuilder)
    {
        return configurationBuilder.AddJsonFile("appsettings.json", true, true);
    }
}