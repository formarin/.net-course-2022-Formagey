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
               .RuleFor(e => e.Salary, f => f.Random.Int(500, 5000));

            return employee.Generate(1000);
        }
        public List<Client> GetClientList()
        {
            var client = new Faker<Client>("ru")
               .RuleFor(c => c.FirstName, f => f.Person.FirstName)
               .RuleFor(c => c.LastName, f => f.Person.LastName)
               .RuleFor(c => c.DateOfBirth, f => f.Person.DateOfBirth)
               .RuleFor(c => c.PhoneNumber, f => 77700000 + f.UniqueIndex);

            return client.Generate(1000);
        }
        public Dictionary<int, Client> GetClientDictionary()
        {
            return GetClientList().ToDictionary(keySelector: client => client.PhoneNumber, elementSelector: client => client);
        }
    }
}
