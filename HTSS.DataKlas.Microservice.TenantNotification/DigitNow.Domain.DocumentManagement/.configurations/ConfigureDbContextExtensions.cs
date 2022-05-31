using System;
using System.Reflection;
using HTSS.Platform.Infrastructure.Data.EntityFramework;
using HTSS.Platform.Infrastructure.Data.EntityFramework.MicrosoftExtensions;
using HTSS.Platform.Infrastructure.Data.EntityFramework.MultiTenant;
using HTSS.Platform.Infrastructure.Data.EntityFramework.MultiTenant.MicrosoftExtensions;
using HTSS.Platform.Infrastructure.MultiTenant;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShiftIn.Domain.TenantNotification.Data;

namespace ShiftIn.Domain.TenantNotification
{
    internal static class ConfigureDbContextExtensions
    {
        private const string _applicationConfigurationDbContextSection = "TenantNotificationConnectionString:TenantNotification";
        private static readonly Assembly _currentAssembly = typeof(DomainServiceExtensions).Assembly;

        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>(MultiTenantOptions.EnableMultiTenant))
            {
                var appConfigOptions = configuration.GetSection(_applicationConfigurationDbContextSection).Get<EfMultiTenantDbOptions>();
                services.AddMultiTenantEntityFramework<TenantNotificationDbContext>(options =>
                {
                    options.Options = appConfigOptions.Options;
                    options.DbProvider = appConfigOptions.DbProvider;
                    options.DbContextAssembly = _currentAssembly;
                }, (options, serviceProvider) =>
                {
                    var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                    if (loggerFactory == null) return;
                    var logger = loggerFactory.CreateLogger<IServiceCollection>();

                    var efTenantService = serviceProvider.GetService<IEfTenantService>();
                    logger.LogDebug("Trying to identity the tenant...");

                    if (efTenantService != null)
                    {
                        var tenantInfo = efTenantService.GetTenantInfo();

                        if (tenantInfo is null)
                            throw new NullReferenceException("Tenant Info Configurations not found for current context");

                        logger.LogDebug("Using tenant information from current session with tenant id: {TenantId}", tenantInfo.TenantId);

                        options.TenantCode = tenantInfo.TenantCode;
                        options.TenantId = tenantInfo.TenantId;
                        options.TenantName = tenantInfo.TenantName;

                        logger.LogDebug("Using tenant information {.TenantName}, {.TenantName}, {.TenantId}", tenantInfo.TenantName, tenantInfo.TenantName, tenantInfo.TenantId);
                    }
                    else
                    {
                        throw new NullReferenceException("Can not identity an instance of the IEfTenantService service");
                    }
                }, options =>
                {
                    if (options is SqlServerDbContextOptionsBuilder mssqlOptions)
                        mssqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", TenantNotificationDbContext.Schema);
                });
                return services;
            }
            else
            {
                var appConfigOptions = configuration.GetSection(_applicationConfigurationDbContextSection).Get<EFDbOptions>();
                services.AddEntityFramework<TenantNotificationDbContext>(options =>
                {
                    options.ConnectionString = appConfigOptions.ConnectionString;
                    options.DbProvider = appConfigOptions.DbProvider;
                    options.DbContextAssembly = _currentAssembly;
                }, options =>
                {
                    if (options is SqlServerDbContextOptionsBuilder mssqlOptions)
                        mssqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", TenantNotificationDbContext.Schema);
                });
                return services;
            }
        }
    }
}