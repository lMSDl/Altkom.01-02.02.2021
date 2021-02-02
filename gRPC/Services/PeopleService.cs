using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using gRPC;
using Grpc.Core;
using Services.Interfaces;

namespace gRPC.Services
{
    public class PeopleService : GrpcPeopleService.GrpcPeopleServiceBase
    {

        private IService<Models.Person> Service {get;}
        private IMapper Mapper {get;}

        public PeopleService(IService<Models.Person> service, IMapper mapper)
        {
            Service = service;
            Mapper = mapper;
        }

        public async override Task<Person> ReadById(Person request, ServerCallContext context)
        {
            var person = await Service.ReadAsync(request.Id);
            return Mapper.Map<Person>(person);
        }

        public async override Task<People> Read(None request, ServerCallContext context)
        {
            var peopleCollection = await Service.ReadAsync();
            var people = new People();
            people.Collection.AddRange(peopleCollection.Select(x => Mapper.Map<Person>(x)));
            return people;
        }

        public async override Task<Person> Create(Person request, ServerCallContext context)
        {
            var person = await Service.CreateAsync(Mapper.Map<Models.Person>(request));
            return Mapper.Map<Person>(person);
        }

        public async override Task<None> Delete(Person request, ServerCallContext context)
        {
            await Service.DeleteAsync(request.Id);
            return new None();
        }

        public async override Task<None> Update(Person request, ServerCallContext context)
        {
            await Service.UpdateAsync(request.Id, Mapper.Map<Models.Person>(request));
            return new None();
        }
    }
}