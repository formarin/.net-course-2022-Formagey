using Models;
using Services.Exceptions;
using Services.Filters;
using Services.Storages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class ClientService<T> where T : IClientStorage
    {
        public T _clientStorage;

        public ClientService(T clientStorage)
        {
            _clientStorage = clientStorage;
        }

        public void AddClient(Client client)
        {
            if (DateTime.Now.Year - client.DateOfBirth.Year < 18)
            {
                throw new AgeLimitException("Минимально допустимый возраст: 18 лет.");
            }
            if (client.FirstName == null | client.LastName == null | client.LastName == null |
                client.PassportNumber == 0 | client.DateOfBirth == new DateTime(0))
            {
                throw new NoPassportDataException("Наличие всех паспортных данных обязательно.");
            }

            _clientStorage.Add(client);
        }

        public void UpdateClient(Client client)
        {
            _clientStorage.Update(client);
        }

        public void DeleteClient(Client client)
        {
            _clientStorage.Delete(client);
        }

        public void AddAccount(Client client, Account account)
        {
            _clientStorage.AddAccount(client, account);
        }

        public void UpdateAccount(Client client, Account account)
        {
            _clientStorage.UpdateAccount(client, account);
        }
        public void DeleteAccount(Client client, Account account)
        {
            _clientStorage.DeleteAccount(client, account);
        }

        public Dictionary<Client, List<Account>> GetClients(ClientFilter filter)
        {
            var query = _clientStorage.Data.Where(_ => true);

            if (filter.FirstName != null)
                query = query.Where(x => x.Key.FirstName == filter.FirstName);

            if (filter.LastName != null)
                query = query.Where(x => x.Key.LastName == filter.LastName);

            if (filter.PhoneNumber != null)
                query = query.Where(x => x.Key.PhoneNumber == filter.PhoneNumber);

            if (filter.PassportNumber != null)
                query = query.Where(x => x.Key.PassportNumber == filter.PassportNumber);

            if (filter.MinDate != null)
                query = query.Where(x => x.Key.DateOfBirth >= filter.MinDate);

            if (filter.MaxDate != null)
                query = query.Where(x => x.Key.DateOfBirth <= filter.MaxDate);

            return query.ToDictionary(keySelector: x => x.Key, elementSelector: x => x.Value);
        }
    }
}
