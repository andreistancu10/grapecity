using System;
using DigitNow.Domain.DocumentManagement.configurations;
using DigitNow.Microservice.DocumentManagement.configurations;
using DigitNow.Microservice.DocumentManagement.configurations.Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DigitNow.Microservice.DocumentManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = SerilogConfigurationExtensions.BuildConfiguration();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();
            try
            {
                Log.Information("Application Starting.");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The Application failed to start.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureAppConfiguration((host, config) => config
                    .ConfigureApi(host.HostingEnvironment)
                    .ConfigureTenantNotificationDomain(host.HostingEnvironment))
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}