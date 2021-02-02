using System;
using System.ServiceProcess;

namespace WindowsService.Microsoft
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBase.Run(new LoggingService());
        }
    }
}
