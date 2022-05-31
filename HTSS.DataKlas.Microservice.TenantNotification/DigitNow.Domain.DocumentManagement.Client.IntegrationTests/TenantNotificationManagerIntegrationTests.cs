using FluentAssertions;
using HTSS.Platform.Infrastructure.MultiTenant;
using HTSS.Platform.Infrastructure.MultiTenant.MicrosoftExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShiftIn.Domain.TenantNotification.Client.configurations;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications.CreateNotification;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ShiftIn.Domain.TenantNotification.Client.IntegrationTests
{
    public class TenantNotificationManagerIntegrationTests : IDisposable
    {
        private readonly CancellationToken _cancellationToken;
        private readonly ITenantNotificationManager _tenantNotificationManager;
        private readonly IHostedService _configureApiBusHostedService;
        private readonly TenantInfoLoader _tenantInfoLoader;
        private readonly ITenantService _tenantService;

        public TenantNotificationManagerIntegrationTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddMassTransitConfigurations(configuration);
            serviceCollection.AddTenantNotificationMQServices();
            serviceCollection.AddLogging(configure => configure.AddConsole());
            serviceCollection.AddMultiTenant();

            _cancellationToken = new CancellationToken();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            _configureApiBusHostedService = serviceProvider.GetService<IHostedService>();
            Task.Run(() => _configureApiBusHostedService.StartAsync(_cancellationToken)).ConfigureAwait(false);

            _tenantNotificationManager = serviceProvider.GetService<ITenantNotificationManager>();

            _tenantInfoLoader = serviceProvider.GetService<TenantInfoLoader>();

            _tenantService = serviceProvider.GetService<ITenantService>();
            _tenantService.SetTenantInfo(_tenantInfoLoader.GetTenants().First().TenantId);
        }

        public void Dispose()
        {
            Task.Run(() => _configureApiBusHostedService.StopAsync(_cancellationToken)).ConfigureAwait(false);
        }

        [Fact]
        public async Task CreateNotification()
        {
            await _tenantNotificationManager.CreateNotification(new CreateNotificationEvent
            {
                NotificationTypeId = NotificationTypeEnum.ConfiguratorEmployeeMobilityTypesNotification,
                FromUserId = 2,
                EntityId = 0,
                UserId = 2,
                Params = new List<INotificationEventParam>(),
            }, _cancellationToken);
        }
    }
}