using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models;
using Services.Interfaces;
using WebApi.Hubs;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        
        private IService<Person> Service {get;}
        private IHubContext<PeopleHub> Hub {get;}
        public PeopleController(IService<Person> service, IHubContext<PeopleHub> hubContext)
        {
            Service = service;
            Hub = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            return Ok(await Service.ReadAsync());
        }

        [HttpGet("{id:int}", Name = nameof(GetById))]
        public async Task<IActionResult> GetById(int id) {
            return Ok(await Service.ReadAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Person entity) {
            entity = await Service.CreateAsync(entity);

            await Hub.Clients.Group("Add").SendAsync(nameof(Post), entity);

            return CreatedAtRoute(nameof(GetById), new {Id = entity.Id}, entity);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, Person entity) {
            if(await Service.ReadAsync(id) == null)
                return NotFound();

            await Service.UpdateAsync(id, entity);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) {
            await Service.DeleteAsync(id);
            await Hub.Clients.Group("Delete").SendAsync(nameof(Delete), $"Person:{id} deleted");
            return NoContent();
        }
    }
}