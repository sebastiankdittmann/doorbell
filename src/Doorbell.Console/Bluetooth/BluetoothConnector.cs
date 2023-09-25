using System.Net.Sockets;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Doorbell.Console.Bluetooth
{
    public class BluetoothConnector : IBluetoothConnector
    {
        private readonly ILogger<BluetoothConnector> _logger;
        private readonly BluetoothOptions _options;

        public BluetoothConnector(ILogger<BluetoothConnector> logger, IOptions<BluetoothOptions> options)
        {            
            _logger = logger;
            _options = options.Value;
        }

        public async Task<bool> ConnectAsync()
        {
            try
            {
                var address = BluetoothAddress.Parse(_options.DeviceAddress);
                var client = new BluetoothClient();
                var device = new BluetoothDeviceInfo(address);

                if (device.Authenticated)
                {
                    await client.ConnectAsync(address, BluetoothService.SerialPort);
                    client.Close();
                    return true;
                }

                return false;
            }
            catch (SocketException ex)
            {
                _logger.LogError("Failed to connect to Bluetooth device: {Message}", ex.Message);
                return false;
            }
        }
    }
}