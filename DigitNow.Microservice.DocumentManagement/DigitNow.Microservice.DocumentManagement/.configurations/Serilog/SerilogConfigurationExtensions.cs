using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DigitNow.Microservice.DocumentManagement.configurations.Serilog
{
    public static class SerilogConfigurationExtensions
    {
        public static IConfigurationRoot BuildConfiguration()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string appSettings = Environments.Development.Equals(environment, StringComparison.OrdinalIgnoreCase)
                ? "appsettings.Development.json"
                : "appsettings.json";

            return new ConfigurationBuilder().AddJsonFile(appSettings).Build();
        }
    }
}