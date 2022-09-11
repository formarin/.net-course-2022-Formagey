using Bogus;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class TestDataGenerator
    {
        public List<Employee> GetEmployeeList()
        {
            var employee = new Faker<Employee>("ru")
               .RuleFor(e => e.FirstName, f => f.Person.FirstName)
               .RuleFor(e => e.LastName, f => f.Person.LastName)
               .RuleFor(e => e.DateOfBirth, f => f.Person.DateOfBirth)
               .RuleFor(e => e.Salary, f => f.Random.Int(500, 5000))
               .RuleFor(e => e.PassportNumber, f => int.MaxValue - f.UniqueIndex);

            return employee.Generate(1000);
        }
        public List<Client> GetClientList()
        {
            var client = new Faker<Client>("ru")
               .RuleFor(c => c.FirstName, f => f.Person.FirstName)
               .RuleFor(c => c.LastName, f => f.Person.LastName)
               .RuleFor(c => c.DateOfBirth, f => f.Person.DateOfBirth)
               .RuleFor(c => c.PhoneNumber, f => 77700000 + f.UniqueIndex)
               .RuleFor(c => c.PassportNumber, f => int.MaxValue - f.UniqueIndex);

            return client.Generate(1000);
        }
        public Dictionary<int, Client> GetClientDictionary()
        {
            return GetClientList().ToDictionary(
                keySelector: client => client.PhoneNumber,
                elementSelector: client => client);
        }
        public Dictionary<Client, Account[]> GetClientAndAccountDictionary()
        {
            var faker = new Faker();
            var currencies = new Account[]
                {
                    new Account
                    {
                        Amount = faker.Random.UInt(),
                        Currency = new Currency{ Code = 498, Name = "MDL"}
                    },
                    new Account
                    {
                        Amount = faker.Random.UInt(),
                        Currency = new Currency{ Code = 643, Name = "RUB"}
                    },
                    new Account
                    {
                        Amount = faker.Random.UInt(),
                        Currency = new Currency{ Code = 840, Name = "USD"}
                    },
                    new Account
                    {
                        Amount = faker.Random.UInt(),
                        Currency = new Currency{ Code = 978, Name = "EUR"}
                    }
                };
            return GetClientList().ToDictionary(keySelector: client => client,
                elementSelector: client => faker.Random.ListItems(currencies, faker.Random.Int(1, currencies.Length)).ToArray());
        }
    }
}
