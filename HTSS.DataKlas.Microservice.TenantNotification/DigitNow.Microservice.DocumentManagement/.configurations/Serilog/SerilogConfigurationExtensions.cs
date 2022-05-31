using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Microservice.TenantNotification
{
    public static class SerilogConfigurationExtensions
    {
        public static IConfigurationRoot BuildConfiguration()
        {
            string enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string appSettings = Environments.Development.Equals(enviroment, StringComparison.OrdinalIgnoreCase)
                ? "appsettings.Development.json"
                : "appsettings.json";

            return new ConfigurationBuilder().AddJsonFile(appSettings).Build();
        }
    }
}