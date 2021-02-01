using Bogus;

namespace Models.Fakers
{
    public abstract class EntityFaker<T> : Faker<T> where T : Entity
    {
        protected EntityFaker() : base("pl")
        {
            StrictMode(true);
            RuleFor(x => x.Id, f => f.UniqueIndex);
        }
    }
}