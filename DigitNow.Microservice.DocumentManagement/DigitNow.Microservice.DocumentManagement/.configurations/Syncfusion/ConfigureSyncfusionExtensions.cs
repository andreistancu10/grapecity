using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Syncfusion.Licensing;

namespace DigitNow.Microservice.DocumentManagement.configurations.Syncfusion
{
    public static class ConfigureSyncfusionExtensions
    {
        public static IApplicationBuilder AddSyncfusionConfigurations(this IApplicationBuilder app, IConfiguration configuration)
        {
            var licenseKey = configuration.GetValue<string>("Syncfusion:LicenseKey");
            SyncfusionLicenseProvider.RegisterLicense(licenseKey);
            return app;
        }
    }
}
