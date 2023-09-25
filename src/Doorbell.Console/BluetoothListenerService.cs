using Doorbell.Console.Audio;
using Doorbell.Console.Bluetooth;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Doorbell.Console
{
    public partial class BluetoothListenerService : BackgroundService
    {
        private readonly ILogger<BluetoothListenerService> _logger;
        private readonly IBluetoothConnector _connector;
        private readonly IAudioPlayer _player;

        public BluetoothListenerService(ILogger<BluetoothListenerService> logger, IBluetoothConnector connector, IAudioPlayer player)
        {
            _logger = logger;
            _connector = connector;
            _player = player;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Connect to Bluetooth device here
                if (await _connector.ConnectAsync())
                {
                    _logger.LogInformation("Bluetooth client connected");

                    _player.PlaySound();
                }
                else
                {
                    _logger.LogInformation("Bluetooth client not connected");
                }
            }
        }
    }
}