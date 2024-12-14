using Bogus;
using Bogus.Extensions.UnitedStates;
using People.Application.Repositories.Common;
using Person = People.Domain.Entities.Person;

namespace People.Infrastructure.Persistence.Seeds;

public class PeopleContextSeed(IRepository<Person> repository)
{
    public async Task EnsureCreatedAsync()
    {
        await repository.Context.Database.EnsureCreatedAsync();
    }
    
    public async Task EnsureDeletedAsync()
    {
        await repository.Context.Database.EnsureDeletedAsync();
        await repository.Context.DisposeAsync();
    }
    
    public async Task SeedAsync()
    {
        if (await repository.AnyAsync() == true)
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

        await repository.SaveChangesAsync();
    }
}