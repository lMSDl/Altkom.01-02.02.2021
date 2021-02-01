using Bogus;

namespace Models.Fakers
{
    public class PersonFaker : EntityFaker<Person>
    {
        public PersonFaker() {
            RuleFor(x => x.FirstName,  f => f.Person.FirstName);
            RuleFor(x => x.LastName,  f => f.Person.LastName);
        }
    }
}