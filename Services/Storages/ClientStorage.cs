using Models;
using Services.Storages;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class ClientStorage : IClientStorage
    {
        public Dictionary<Client, List<Account>> Data { get; }

        public ClientStorage()
        {
            Data = new Dictionary<Client, List<Account>>();
        }

        public void Add(Client client)
        {
            Data.Add(client, new List<Account>
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

        public void Update(Client client)
        {
            var existingClient = Data.Keys.FirstOrDefault(x => x == client);
            existingClient = client;
        }

        public void Delete(Client client)
        {
            Data.Remove(client);
        }

        public void AddAccount(Client client, Account account)
        {
            Data[client].Add(account);
        }

        public void UpdateAccount(Client client, Account account)
        {
            var existingAccount = Data[client].First(x => x == account);
            existingAccount = account;
        }
        public void DeleteAccount(Client client, Account account)
        {
            Data[client].Remove(account);
        }
    }
}
