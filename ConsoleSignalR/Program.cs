using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Models;
using Newtonsoft.Json;

namespace ConsoleSignalR
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HubConnection connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/signalR/People")
            .WithAutomaticReconnect()
            .Build();

            connection.ServerTimeout = TimeSpan.FromSeconds(10);

            connection.Reconnecting += connectionId => {
                System.Console.WriteLine("Reconnecting");
                return Task.CompletedTask;
            };
            connection.Closed += connectionId => {
                System.Console.WriteLine("Closed");
                return Task.CompletedTask;
            };
            
            connection.On<Person>("Post", x => System.Console.WriteLine( JsonConvert.SerializeObject(x) ));
            connection.On<string>("Delete", x => System.Console.WriteLine(x) );
            //connection.On<string>("Pong", x => System.Console.WriteLine(x) );
        

            while(true) {
                System.Console.WriteLine("Connecting...");
                try {
                await connection.StartAsync(); 
                System.Console.WriteLine("Connected");
                break;
                }
                catch {}
            }          

            if(args.Any()) {
                System.Console.WriteLine(args[0]); 
                if( args[0] == "Add")
                    await connection.SendAsync("JoinToAddGroup");
                else if( args[0] == "Delete")
                    await connection.SendAsync("JoinToDeleteGroup");
            }

            await Task.Delay(5000);
            try {
            await connection.SendAsync("addperson", new Person {FirstName = "Ewa", LastName = "Ewowska"});
            }
            catch
            {

            }
            Console.ReadLine();
            await connection.DisposeAsync();
        }
    }
}
