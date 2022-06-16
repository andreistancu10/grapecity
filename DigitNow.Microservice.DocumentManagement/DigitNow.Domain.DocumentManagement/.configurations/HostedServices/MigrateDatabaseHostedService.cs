using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Infrastructure.Data.EntityFramework.MultiTenant;
using HTSS.Platform.Infrastructure.Environment;
using HTSS.Platform.Infrastructure.MultiTenant;
using HTSS.Platform.Infrastructure.MultiTenant.MicrosoftExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DigitNow.Domain.DocumentManagement.configurations.HostedServices
{
    public class MigrateDatabaseHostedService : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MigrateDatabaseHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public MigrateDatabaseHostedService(ILogger<MigrateDatabaseHostedService> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string migrateDatabase = Environment.GetEnvironmentVariable(EnvironmentVariables.MigrateDatebase);
            bool.TryParse(migrateDatabase, out bool applyMigrations);
            if (!applyMigrations)
            {
                _logger.LogInformation("Database migration skipped");
                return;
            }

            try
            {
                if (_configuration.GetValue<bool>(MultiTenantOptions.EnableMultiTenant))
                {
                    var tenantInfoLoader = _serviceProvider.GetRequiredService<TenantInfoLoader>();

                    var tenants = tenantInfoLoader.GetTenants();
                    foreach (var tenant in tenants)
                    {
                        _logger.LogInformation("Starting database migration for tenant: {TenantCode}", tenant.TenantCode);
                        var services = new ServiceCollection();
                        services.AddScoped<IConfiguration>(_ => _configuration);
                        services.AddMultiTenant();
                        services.AddDbContext(_configuration);
                        services.AddLogging();
                        services.AddMediatR(typeof(MigrateDatabaseHostedService).Assembly);

                        await using var serviceProvider = services.BuildServiceProvider();

                        var efTenantService = serviceProvider.GetRequiredService<IEfTenantService>();
                        efTenantService.SetTenantInfo(tenant.TenantId);

                        var dbContext = serviceProvider.GetRequiredService<DocumentManagementDbContext>();
                        if (dbContext.Database.GetMigrations().Any())
                        {
                            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(cancellationToken);
                            var migrations = pendingMigrations.ToList();
                            if (migrations.Count > 0)
                            {
                                _logger.LogInformation("Pending {Count} changes to apply to: {TenantCode}", migrations.Count, tenant.TenantCode);
                                await dbContext.Database.MigrateAsync(cancellationToken);
                                _logger.LogInformation("Database {Count} changes applied to: {TenantCode}", migrations.Count, tenant.TenantCode);
                            }

                            var lastAppliedMigration = (await dbContext.Database.GetAppliedMigrationsAsync(cancellationToken)).LastOrDefault();

                            if (!string.IsNullOrEmpty(lastAppliedMigration))
                                _logger.LogInformation("Migration schema version: {LastAppliedMigration} for tenant: {TenantCode}", lastAppliedMigration, tenant.TenantCode);
                        }
                        _logger.LogInformation("Database migration successfully applied for tenant: {TenantCode}", tenant.TenantCode);
                    }
                }
                else
                {
                    var services = new ServiceCollection();

                    services.AddDbContext(_configuration);
                    services.AddMediatR(typeof(MigrateDatabaseHostedService).Assembly);

                    await using var serviceProvider = services.BuildServiceProvider();

                    var dbContext = serviceProvider.GetRequiredService<DocumentManagementDbContext>();
                    if (dbContext.Database.GetMigrations().Any())
                    {
                        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(cancellationToken);
                        var migrations = pendingMigrations.ToList();
                        if (migrations.Count > 0)
                        {
                            _logger.LogInformation("Pending {Count} changes to apply", migrations.Count);
                            await dbContext.Database.MigrateAsync(cancellationToken);
                            _logger.LogInformation("Database {Count} changes applied", migrations.Count);
                        }

                        var lastAppliedMigration = (await dbContext.Database.GetAppliedMigrationsAsync(cancellationToken)).LastOrDefault();

                        if (!string.IsNullOrEmpty(lastAppliedMigration))
                            _logger.LogInformation("Migration schema version: {LastAppliedMigration}", lastAppliedMigration);
                    }

                    _logger.LogInformation("Database migration applied");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on applying database migration");
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}