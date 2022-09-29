using Microsoft.Extensions.Configuration;

namespace DigitNow.Microservice.DocumentManagement.configurations.Serilog;

public static class SerilogConfigurationExtensions
{
    public static IConfigurationRoot BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    }
}