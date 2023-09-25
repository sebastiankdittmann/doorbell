using System.Diagnostics;
using System.IO.Abstractions;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Doorbell.Console.Audio
{
    public class AudioPlayer : IAudioPlayer
    {
        private readonly ILogger<AudioPlayer> _logger;
        private readonly AudioOptions _options;

        public AudioPlayer(ILogger<AudioPlayer> logger, IOptions<AudioOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public void PlaySound()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var fileSystem = new FileSystem();
                if (fileSystem.File.Exists(_options.SoundFilePath))
                {
                    using var player = new System.Media.SoundPlayer(_options.SoundFilePath);
                    player.Play();
                    Thread.Sleep(1000);
                    player.Stop();
                }
                else
                {
                    System.Media.SystemSounds.Beep.Play();
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                PlayLinuxAudio();
            }
            else
            {
                _logger.LogError("Unsupported operating system");
            }
        }

        private void PlayLinuxAudio()
        {
            // On Linux, play a system sound using the "aplay" command
            var process = new Process();
            process.StartInfo.FileName = "aplay";
            process.StartInfo.Arguments = _options.SoundFilePath;
            process.Start();
            Thread.Sleep(1000);
            process.Kill();
        }
    }
}