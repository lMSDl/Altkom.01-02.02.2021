using System;
using System.IO;
using Topshelf;

namespace WindowsService.TopShelf
{
    public class LoggingService : ServiceControl
    {
        private const string _filename = "C:\\logs\\topshelfServiceLog.txt";

        public bool Start(HostControl hostControl)
        {
            Log(nameof(Start));
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            Log(nameof(Stop));
            return true;
        }

        private void Log(string @string) {
            Directory.CreateDirectory(Path.GetDirectoryName(_filename));
            File.AppendAllText(_filename, $"{DateTime.UtcNow.ToString()}: {@string}\n");
        }

        
    }
}