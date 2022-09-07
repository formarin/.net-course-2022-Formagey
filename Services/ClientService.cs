using Models;
using Services.Exceptions;
using System;
using System.Collections.Generic;

namespace Services
{
    public class ClientService
    {
        private Dictionary<Client, List<Account>> _clientDictionary = new();
        public void AddClient(Client client)
        {
            if (DateTime.Now.Year - client.DateOfBirth.Year < 18)
            {
                throw new AgeLimitException("Минимально допустимый возраст: 18 лет.");
            }
            if (client.FirstName == null || client.LastName == null || client.DateOfBirth == new DateTime(0))
            {
                throw new NoPassportDataException("Наличие всех паспортных данных обязательно.");
            }

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
}
