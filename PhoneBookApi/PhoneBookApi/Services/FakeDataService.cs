using Bogus;
using PhoneBookApi.Data;
using PhoneBookApi.Models;

public class FakeDataGenerator
{
    private readonly ApplicationDbContext _context;

    public FakeDataGenerator(ApplicationDbContext context)
    {
        _context = context;
    }

    public void EnsureData()
    {
        if (!_context.Users.Any())
        {
            var users = GenerateUsers(10);
            _context.Users.AddRange(users);
            _context.SaveChanges();
        }
    }

    private List<User> GenerateUsers(int count)
    {
        var faker = new Faker<User>("ru")
            .RuleFor(u => u.Name, f => f.Name.FirstName())
            .RuleFor(u => u.Surname, f => f.Name.LastName())
            .RuleFor(u => u.PhoneNumber, f => 
                $"+7({f.Random.Int(900, 999)}){f.Random.Int(100, 999)}-{f.Random.Int(10, 99)}-{f.Random.Int(10, 99)}");

        return faker.Generate(count);
    }
}