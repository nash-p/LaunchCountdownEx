using System.IO;
using System.Reflection;

namespace LaunchCountDown.Config
{
    class LaunchCountdownConfig
    {
        private static LaunchCountdownConfig _config;

        private readonly string _configPath;

        private LaunchCountdownConfig()
        {
            var currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var configDir = Path.Combine(currentDir, "config");

            if (!Directory.Exists(configDir))
            {
                Directory.CreateDirectory(configDir);
            }

            _configPath = Path.Combine(configDir, "lcdex.cfg");

            Info = new ConfigInfo(_configPath);

            Info.Load();
        }

        internal static LaunchCountdownConfig Instance
        {
            get { return _config ?? (_config = new LaunchCountdownConfig()); }
        }

        public ConfigInfo Info { get; private set; }

    }
}


