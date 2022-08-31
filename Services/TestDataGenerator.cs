using Bogus;
using Models;
using System.Collections.Generic;

namespace Services
{
    public class TestDataGenerator
    {
        public List<Employee> GetEmployeeList()
        {
            var list = new List<Employee>();
            for (int i = 0; i < 1000; i++)
            {
                list.Add(CreateEmployee());
            }
            return list;
        }
        public List<Client> GetClientList()
        {
            var list = new List<Client>();
            for (int i = 0; i < 1000; i++)
            {
                list.Add(CreateClient());
            }
            return list;
        }
        public Dictionary<int, Client> GetClientDictionary()
        {
            var dictionary = new Dictionary<int, Client>();
            for (int i = 0; i < 1000; i++)
            {
                Client client;
                do
                {
                    client = CreateClient();
                }
                while (dictionary.ContainsKey(client.PhoneNumber));
                dictionary.Add(client.PhoneNumber, client);
            }
            return dictionary;
        }
        private Client CreateClient()
        {
            var faker = new Faker();
            return new Client
            {
                FirstName = faker.Person.FirstName,
                LastName = faker.Person.LastName,
                DateOfBirth = faker.Person.DateOfBirth,
                PhoneNumber = faker.Phone.Random.Int(77700000, 77999999)
            };
        }
        private Employee CreateEmployee()
        {
            var faker = new Faker();
            return new Employee
            {
                FirstName = faker.Person.FirstName,
                LastName = faker.Person.LastName,
                DateOfBirth = faker.Person.DateOfBirth,
                Salary = faker.Random.Int(500, 5000)
            };
        }
    }
}
