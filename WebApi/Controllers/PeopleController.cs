using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        
        private IService<Person> Service {get;}
        public PeopleController(IService<Person> service)
        {
            Service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            return Ok(await Service.ReadAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id) {
            return Ok(await Service.ReadAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post(Person entity) {
            entity = await Service.CreateAsync(entity);
            return CreatedAtRoute("default", new {Id = entity.Id}, entity);
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
            return NoContent();
        }
    }
}