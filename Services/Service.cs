using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Models;
using Services.Interfaces;

namespace Services
{
    public class Service<T> : IService<T> where T : Entity
    {
        public Service(Faker<T> faker, int count)
        {
            Entities = faker.Generate(count);
            _index = Entities.Max(x => x.Id);
        }

        private ICollection<T> Entities {get;}
        private int _index;


        public Task<T> CreateAsync(T entity)
        {
            entity.Id = ++_index;
            Entities.Add(entity);
            return Task.FromResult(entity);
        }

        public Task DeleteAsync(int id)
        {
            Entities.Remove(Entities.SingleOrDefault(x => x.Id == id));
            return Task.CompletedTask;
        }

        public Task<IEnumerable<T>> ReadAsync()
        {
            return Task.FromResult(Entities.ToList().AsEnumerable());
        }

        public Task<T> ReadAsync(int id)
        {
            return Task.FromResult(Entities.SingleOrDefault(x => x.Id == id));
        }

        public async Task UpdateAsync(int id, T entity)
        {
            entity.Id = id;
            var dbEntity = await ReadAsync(id);
            if(dbEntity != null)
            {
                Entities.Remove(dbEntity);
                Entities.Add(entity);
            }
        }
    }
}
