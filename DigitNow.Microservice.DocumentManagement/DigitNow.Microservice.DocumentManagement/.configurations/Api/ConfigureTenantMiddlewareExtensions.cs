using HTSS.Platform.Infrastructure.MultiTenant;
using HTSS.Platform.Infrastructure.MultiTenant.MicrosoftExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DigitNow.Microservice.DocumentManagement.configurations.Api
{
    public static class ConfigureTenantMiddlewareExtensions
    {
        public static IApplicationBuilder UseMultiTenant(this IApplicationBuilder app)
        {
            var configService = app.ApplicationServices.GetRequiredService<IConfiguration>();
            bool multiTenantEnabled = configService.GetValue<bool>(MultiTenantOptions.EnableMultiTenant);
            var logger = app.ApplicationServices.GetService<ILogger<IApplicationBuilder>>();

            if (multiTenantEnabled)
            {
                app.UseTenantMiddleware();
                logger.LogDebug("Multitenant Middleware: Setting is enabled!");
            }
            else
            {
                logger.LogDebug("Multitenant Middleware: Setting not enabled!");
            }

            return app;
        }
    }
}