using Bogus;
using Bogus.Extensions.UnitedStates;
using People.Application.Repositories.Common;
using Person = People.Domain.Entities.Person;

namespace People.Infrastructure.Persistence.Seeds;

public class BuildMockPeople
{
    public static void Build(IRepository<Person> repository)
    {
        if (repository.Any() == true)
            return;

        const int size = 25;
        var list = new List<Person>(25);

        for (var i = 0; i < size; i++)
        {
            var people = new Faker<Person>()
                .RuleFor(p => p.Fullname, f => f.Person.FullName)
                .RuleFor(p => p.DateOfBirth, f => f.Person.DateOfBirth)
                .RuleFor(p => p.Email, f => f.Person.Email)
                .RuleFor(p => p.PhoneNumber, f => f.Phone.PhoneNumber("+###########"))
                .RuleFor(p => p.Dni, f => f.Person.Ssn());
            
            list.Add(people.Generate());
        }
        
        repository.AddRange(list);

        repository.SaveChanges();
    }
}