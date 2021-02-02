using System;
using System.IO;
using System.ServiceProcess;

namespace WindowsService.Microsoft
{
    public class LoggingService : ServiceBase
    {
        private const string _filename = "C:\\logs\\microsofServiceLog.txt";
        private void Log(string @string) {
            Directory.CreateDirectory(Path.GetDirectoryName(_filename));
            File.AppendAllText(_filename, $"{DateTime.UtcNow.ToString()}: {@string}\n");
        }

        protected override void OnStart(string[] args)
        {
            Log(nameof(OnStart));
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            Log(nameof(OnStop));
            base.OnStop();
        }

        protected override void OnPause()
        {
            Log(nameof(OnPause));
            base.OnPause();
        }

        protected override void OnShutdown()
        {
            Log(nameof(OnShutdown));
            base.OnShutdown();
        }

    }
}