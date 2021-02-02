using System;
using Topshelf;

namespace WindowsService.TopShelf
{
    //uruchomienie z parametrami install / start / stop lub bez
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x => {
                x.Service<LoggingService>();
                x.SetServiceName("TopShelfWindowsService");
                x.EnableServiceRecovery(x => 
                    x.RestartService(TimeSpan.FromSeconds(10))
                    .RestartService(TimeSpan.FromSeconds(20))
                    .RestartService(TimeSpan.FromSeconds(30))
                    .SetResetPeriod(1)
                );
                x.RunAsLocalSystem();
                x.StartAutomatically();
            });
        }
    }
}
