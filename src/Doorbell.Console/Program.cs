using Doorbell.Console.Audio;
using Doorbell.Console.Bluetooth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Doorbell.Console
{

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<AudioOptions>(hostContext.Configuration.GetSection("Audio"));
                    services.Configure<BluetoothOptions>(hostContext.Configuration.GetSection("Bluetooth"));

                    services.AddTransient<IAudioPlayer, AudioPlayer>();
                    services.AddSingleton<IBluetoothConnector, BluetoothConnector>();
                    services.AddHostedService<BluetoothListenerService>();
                });
        }
    }
}