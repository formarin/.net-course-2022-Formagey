using Bogus;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class TestDataGenerator
    {
        public List<Employee> GetEmployeeList(int count)
        {
            var employee = new Faker<Employee>("ru")
               .RuleFor(e => e.Id, f => Guid.NewGuid())
               .RuleFor(e => e.FirstName, f => f.Person.FirstName)
               .RuleFor(e => e.LastName, f => f.Person.LastName)
               .RuleFor(e => e.DateOfBirth, f => f.Person.DateOfBirth)
               .RuleFor(e => e.Salary, f => f.Random.Int(500, 5000))
               .RuleFor(e => e.PhoneNumber, f => 77700000 + f.UniqueIndex)
               .RuleFor(e => e.PassportNumber, f => int.MaxValue - f.UniqueIndex);

            return employee.Generate(count);
        }
        public List<Client> GetClientList(int count)
        {
            var client = new Faker<Client>("ru")
               .RuleFor(c => c.Id, f => Guid.NewGuid())
               .RuleFor(c => c.FirstName, f => f.Person.FirstName)
               .RuleFor(c => c.LastName, f => f.Person.LastName)
               .RuleFor(c => c.DateOfBirth, f => f.Person.DateOfBirth)
               .RuleFor(c => c.PhoneNumber, f => 77700000 + f.UniqueIndex)
               .RuleFor(c => c.PassportNumber, f => int.MaxValue - f.UniqueIndex);

            return client.Generate(count);
        }
        public Dictionary<int, Client> GetClientDictionary(int count)
        {
            return GetClientList(count).ToDictionary(
                keySelector: client => client.PhoneNumber,
                elementSelector: client => client);
        }
        public Dictionary<Client, Account[]> GetClientAndAccountDictionary(int count)
        {
            var faker = new Faker();
            var currencies = new Account[]
                {
                    new Account
                    {
                        Id = Guid.NewGuid(),
                        Amount = faker.Random.UInt(),
                        CurrencyName = "MDL"
                    },
                    new Account
                    {
                        Id = Guid.NewGuid(),
                        Amount = faker.Random.UInt(),
                        CurrencyName = "RUB"
                    },
                    new Account
                    {
                        Id = Guid.NewGuid(),
                        Amount = faker.Random.UInt(),
                        CurrencyName = "USD"
                    },
                    new Account
                    {
                        Id = Guid.NewGuid(),
                        Amount = faker.Random.UInt(),
                        CurrencyName = "EUR"
                    }
                };
            var dict = GetClientList(count).ToDictionary(keySelector: client => client,
                elementSelector: client => faker.Random.ListItems(currencies, faker.Random.Int(1, currencies.Length)).ToArray());
            foreach(var pair in dict)
                for (int i = 0; i < pair.Value.Length; i++)
                    pair.Value[i].ClientId = pair.Key.Id;
            return dict;
        }
    }
}
