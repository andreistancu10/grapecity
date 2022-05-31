using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DigitNow.Microservice.DocumentManagement.configurations.HostedServices
{
    public class ConfigureApiBusHostedService : IHostedService
    {
        private readonly IBusControl _busControl;
        private readonly ILogger<ConfigureApiBusHostedService> _logger;

        public ConfigureApiBusHostedService(ILogger<ConfigureApiBusHostedService> logger, IBusControl busControl)
        {
            _logger = logger;
            _busControl = busControl;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _busControl.StartAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _busControl.StopAsync(cancellationToken);
        }
    }
}