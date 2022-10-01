using Models;
using Services;
using Services.Exceptions;
using Services.Filters;
using System;
using System.Linq;
using Xunit;

namespace ServiceTests
{
    public class ClientServiceTests
    {
        [Fact]
        public void AddClient_PositiveTest()
        {
            //Arrange
            var clientService = new ClientService();
            var client = new Client
            {
                Id = Guid.NewGuid(),
                FirstName = "firstName",
                LastName = "lastName",
                PhoneNumber = 77700000,
                PassportNumber = 900000000,
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            //Act
            clientService.AddClient(client);

            //Assert
            Assert.Equal(client, clientService.GetClient(client.Id));
        }

        [Fact]
        public void AddClient_throwsAgeLimitException()
        {
            //Arrange
            var clientService = new ClientService();
            var client = new Client
            {
                Id = Guid.NewGuid(),
                FirstName = "firstName",
                LastName = "lastName",
                PhoneNumber = 77700000,
                PassportNumber = 900000000,
                DateOfBirth = new DateTime(2020, 1, 1)
            };

            //Act Assert
            Assert.Throws<AgeLimitException>(() => clientService.AddClient(client));
        }

        [Fact]
        public void AddClient_throwsNoPassportDataException()
        {
            //Arrange
            var clientService = new ClientService();
            var client = new Client
            {
                Id = Guid.NewGuid(),
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            //Act Assert
            Assert.Throws<NoPassportDataException>(() => clientService.AddClient(client));
        }

        [Fact]
        public void GetClients_filteredByFirstName()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientService = new ClientService();
            clientService.AddClientList(testDataGenerator.GetClientList(100));
            var filterByFirstName = new ClientFilter()
            {
                FirstName = "Ирина"
            };

            //Act
            var filtredClients = clientService.GetClients(filterByFirstName);

            //Assert
            Assert.True(filtredClients.All(x => x.FirstName.Contains("Ирина")));
        }

        [Fact]
        public void GetClients_filteredByLastName()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientService = new ClientService();
            clientService.AddClientList(testDataGenerator.GetClientList(100));
            var filterByLastName = new ClientFilter()
            {
                LastName = "Васильев"
            };

            //Act
            var filtredClients = clientService.GetClients(filterByLastName);

            //Assert
            Assert.True(filtredClients.All(x => x.LastName.Contains("Васильев")));
        }

        [Fact]
        public void GetClients_filteredByPhoneNumber()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientService = new ClientService();
            clientService.AddClientList(testDataGenerator.GetClientList(100));
            var filterByPhoneNumber = new ClientFilter()
            {
                PhoneNumber = 77700077
            };

            //Act
            var filtredClients = clientService.GetClients(filterByPhoneNumber);

            //Assert
            Assert.True(filtredClients.All(x => x.PhoneNumber.ToString().Contains(77700077.ToString())));
        }

        [Fact]
        public void GetClients_filteredByPassportNumber()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientService = new ClientService();
            clientService.AddClientList(testDataGenerator.GetClientList(100));
            var filterByPassportNumber = new ClientFilter()
            {
                PassportNumber = 900000000
            };

            //Act
            var filtredClients = clientService.GetClients(filterByPassportNumber);

            //Assert
            Assert.True(filtredClients.All(x => x.PassportNumber.ToString().Contains(900000000.ToString())));
        }

        [Fact]
        public void GetClients_filteredByDateOfBirth()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientService = new ClientService();
            clientService.AddClientList(testDataGenerator.GetClientList(100));
            var filterByDateOfBirth = new ClientFilter()
            {
                MinDate = new DateTime(1990, 1, 1),
                MaxDate = new DateTime(2000, 1, 1)
            };

            //Act
            var filtredClients = clientService.GetClients(filterByDateOfBirth);

            //Assert
            Assert.True(filtredClients.All(x =>
            x.DateOfBirth >= new DateTime(1990, 1, 1) &&
            x.DateOfBirth <= new DateTime(2000, 1, 1)));
        }

        [Fact]
        public void GetClients_filteredWithPagination()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientService = new ClientService();
            clientService.AddClientList(testDataGenerator.GetClientList(100));
            var filterWithPagination = new ClientFilter()
            {
                pageNumber = 3,
                notesCount = 10
            };

            //Act
            var filtredClients = clientService.GetClients(filterWithPagination);

            //Assert
            Assert.Equal(10, filtredClients.Count);
        }

        [Fact]
        public void Delete_PositiveTest()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientService = new ClientService();
            var client = new Client
            {
                Id = Guid.NewGuid(),
                FirstName = "firstName",
                LastName = "lastName",
                PhoneNumber = 77800000,
                PassportNumber = 800000000,
                DateOfBirth = new DateTime(2000, 1, 1)
            };
            clientService.AddClient(client);

            //Act
            clientService.DeleteClient(client.Id);

            //Asssert
            Assert.Null(clientService.GetClient(client.Id));
        }

        [Fact]
        public void Update_PositiveTest()
        {
            //Arrange
            var clientService = new ClientService();
            var client = new Client
            {
                Id = Guid.NewGuid(),
                FirstName = "firstName",
                LastName = "lastName",
                PhoneNumber = 77800000,
                PassportNumber = 800000002,
                DateOfBirth = new DateTime(2000, 1, 1)
            };
            clientService.AddClient(client);
            client.FirstName = "Саша";

            //Act
            clientService.UpdateClient(client);

            //Assert
            Assert.Equal("Саша", clientService.GetClient(client.Id).FirstName);
        }

        [Fact]
        public void AddAccount_PositiveTest()
        {
            //Arrange
            var clientService = new ClientService();
            var client = clientService.GetClients(new ClientFilter
            {
                PassportNumber = 900000000
            }).FirstOrDefault();
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Amount = 0,
                CurrencyName = "RUP"
            };

            //Act
            clientService.AddAccount(client.Id, account);

            //Assert
            Assert.Contains(account, clientService.GetClient(client.Id).AccountCollection);
        }

        [Fact]
        public void UpdateAccount_PositiveTest()
        {
            //Arrange
            var clientService = new ClientService();
            var client = new Client
            {
                Id = Guid.NewGuid(),
                FirstName = "firstName",
                LastName = "lastName",
                PhoneNumber = 77800000,
                PassportNumber = 700000000,
                DateOfBirth = new DateTime(2000, 1, 1)
            };
            clientService.AddClient(client);
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Amount = 0,
                CurrencyName = "RUP"
            };
            clientService.AddAccount(client.Id, account);
            account.Amount += 100;
            var amount = account.Amount;

            //Act
            clientService.UpdateAccount(client.Id, account);

            //Assert
            Assert.Contains(account, clientService.GetClient(client.Id).AccountCollection);
        }

        [Fact]
        public void DeleteAccount_PositiveTest()
        {
            //Arrange
            var clientService = new ClientService();
            var client = new Client
            {
                Id = Guid.NewGuid(),
                FirstName = "firstName",
                LastName = "lastName",
                PhoneNumber = 77800000,
                PassportNumber = 700000000,
                DateOfBirth = new DateTime(2000, 1, 1)
            };
            clientService.AddClient(client);
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Amount = 0,
                CurrencyName = "RUP"
            };
            clientService.AddAccount(client.Id, account);

            //Act
            clientService.DeleteAccount(client.Id, account);

            //Assert
            Assert.DoesNotContain(account, clientService.GetClient(client.Id).AccountCollection);
        }
    }
}
