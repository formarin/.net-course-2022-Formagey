using Models;
using System.Collections.Generic;

namespace Services
{
    public class ClientStorage : IStorage
    {
        public readonly Dictionary<Client, List<Account>> _clientDictionary = new();
        public void Add(Person person)
        {
            var client = person as Client;
            _clientDictionary.Add(client, new List<Account>
            {
                new Account
                {
                    Amount = 0,
                    Currency = new Currency
                    {
                        Name = "USD",
                        Code = 840
                    }
                }
            });
        }
        public void Add(List<Client> clientList)
        {
            foreach (var client in clientList)
            {
                _clientDictionary.Add(client, new List<Account>
                {
                    new Account
                    {
                        Amount = 0,
                        Currency = new Currency
                        {
                            Name = "USD",
                            Code = 840
                        }
                    }
                });
            }
        }
        public void Update(Person person)
        {

        }
        public void Delete(Person person)
        {

        }
    }
}
