using System;
using System.ServiceProcess;

namespace WindowsService.Microsoft
{
    //sc create [nazwa serwisu] BinPath=[ścieżka do pliku exe]
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBase.Run(new LoggingService());
        }
    }
}
