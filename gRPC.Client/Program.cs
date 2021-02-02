using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Newtonsoft.Json;

namespace gRPC.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using(var channel = GrpcChannel.ForAddress("http://localhost:5000")) {
                var client = new GrpcPeopleService.GrpcPeopleServiceClient(channel);

                var people = await client.ReadAsync(new None());
                people.Collection.ToList().ForEach(x => System.Console.WriteLine(JsonConvert.SerializeObject(x)));

                await client.DeleteAsync(new Person {Id = 0});
                people = await client.ReadAsync(new None());
                people.Collection.ToList().ForEach(x => System.Console.WriteLine(JsonConvert.SerializeObject(x)));

                var person = people.Collection.Skip(1).Take(1).Single();
                person.Id = 1;
                await client.UpdateAsync(person);

                people = await client.ReadAsync(new None());
                people.Collection.ToList().ForEach(x => System.Console.WriteLine(JsonConvert.SerializeObject(x)));

            }

        }
    }
}
