using Models;
using Services.Storages;
using System.Collections.Generic;

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
            Data.Add(client, new List<Account> { new Account() });
        }

        public void Add(List<Client> clientList)
        {
            foreach (var client in clientList)
            {
                Data.Add(client, new List<Account> { new Account() });
            }
        }

        public void Update(Client client)
        {
            var accountList = Data[client];
            Data.Remove(client);
            Data.Add(client, accountList);
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
            Data[client].Remove(account);
            Data[client].Add(account);
        }
        public void DeleteAccount(Client client, Account account)
        {
            Data[client].Remove(account);
        }
    }
}
