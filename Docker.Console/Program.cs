using System;
using System.Threading.Tasks;

namespace Docker.Console
{
    class Program
    {
        //docker build -t counter-image -f Dockerfile .
        //docker create --name core-counter counter-image
        //docker start core-counter
        //docker attach --sig-proxy=false core-counter   
        static async Task  Main(string[] args)
        {
            var counter = 0;
            var max = args.Length != 0 ? Convert.ToInt32(args[0]) : -1;
            while(max == -1 || counter < max) {
                System.Console.WriteLine($"Conuter: {++counter}");
                await Task.Delay(1000);
            }
        }
    }
}
