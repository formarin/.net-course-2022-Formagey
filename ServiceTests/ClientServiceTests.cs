using Models;
using Services;
using Services.Exceptions;
using Services.Filters;
using Services.Storages;
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
            var clientStorage = new ClientStorage();
            var clientService = new ClientService<IClientStorage>(clientStorage);
            var client = new Client
            {
                FirstName = "firstName",
                LastName = "lastName",
                PhoneNumber = 77700000,
                PassportNumber = 900000000,
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            //Act
            clientService.AddClient(client);

            //Assert
            Assert.Contains(client, clientStorage.Data.Keys);
        }

        [Fact]
        public void AddClient_throwsAgeLimitException()
        {
            //Arrange
            var clientService = new ClientService<IClientStorage>(new ClientStorage());
            var client = new Client
            {
                DateOfBirth = new DateTime(2020, 1, 1)
            };

            //Act Assert
            Assert.Throws<AgeLimitException>(() => clientService.AddClient(client));
        }

        [Fact]
        public void AddClient_throwsNoPassportDataException()
        {
            //Arrange
            var clientService = new ClientService<IClientStorage>(new ClientStorage());
            var client = new Client
            {
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            //Act Assert
            Assert.Throws<NoPassportDataException>(() => clientService.AddClient(client));
        }

        [Fact]
        public void AddClient_throwsClientAlreadyExistsException()
        {
            //Arrange
            var clientService = new ClientService<IClientStorage>(new ClientStorage());
            var client = new Client
            {
                FirstName = "firstName",
                LastName = "lastName",
                PhoneNumber = 77700000,
                PassportNumber = 900000000,
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            //Act
            clientService.AddClient(client);

            //Assert
            Assert.Throws<ArgumentException>(() => clientService.AddClient(client));
        }

        [Fact]
        public void GetClients_filteredByFirstName()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientStorage = new ClientStorage();
            var clientService = new ClientService<IClientStorage>(clientStorage);
            clientService.AddClientList(testDataGenerator.GetClientList(1000));
            var filterByFirstName = new ClientFilter()
            {
                FirstName = "Ирина"
            };

            //Act
            var filtredClientDictionary = clientService.GetClients(filterByFirstName);

            //Assert
            Assert.True(filtredClientDictionary.All(x => x.Key.FirstName == "Ирина"));
        }

        [Fact]
        public void GetClients_filteredByLastName()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientStorage = new ClientStorage();
            var clientService = new ClientService<IClientStorage>(clientStorage);
            clientService.AddClientList(testDataGenerator.GetClientList(1000));
            var filterByLastName = new ClientFilter()
            {
                LastName = "Васильев"
            };

            //Act
            var filtredClientDictionary = clientService.GetClients(filterByLastName);

            //Assert
            Assert.True(filtredClientDictionary.All(x => x.Key.LastName == "Васильев"));
        }

        [Fact]
        public void GetClients_filteredByPhoneNumber()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientStorage = new ClientStorage();
            var clientService = new ClientService<IClientStorage>(clientStorage);
            clientService.AddClientList(testDataGenerator.GetClientList(1000));
            var filterByPhoneNumber = new ClientFilter()
            {
                PhoneNumber = 77700077
            };

            //Act
            var filtredClientDictionary = clientService.GetClients(filterByPhoneNumber);

            //Assert
            Assert.True(filtredClientDictionary.All(x => x.Key.PhoneNumber == 77700077));
        }

        [Fact]
        public void GetClients_filteredByPassportNumber()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientStorage = new ClientStorage();
            var clientService = new ClientService<IClientStorage>(clientStorage);
            clientService.AddClientList(testDataGenerator.GetClientList(1000));
            var filterByPassportNumber = new ClientFilter()
            {
                PassportNumber = 900000000
            };

            //Act
            var filtredClientDictionary = clientService.GetClients(filterByPassportNumber);

            //Assert
            Assert.True(filtredClientDictionary.All(x => x.Key.PassportNumber == 900000000));
        }

        [Fact]
        public void GetClients_filteredByDateOfBirth()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientStorage = new ClientStorage();
            var clientService = new ClientService<IClientStorage>(clientStorage);
            clientService.AddClientList(testDataGenerator.GetClientList(1000));
            var filterByDateOfBirth = new ClientFilter()
            {
                MinDate = new DateTime(1990, 1, 1),
                MaxDate = new DateTime(2000, 1, 1)
            };

            //Act
            var filtredClientDictionary = clientService.GetClients(filterByDateOfBirth);

            //Assert
            Assert.True(filtredClientDictionary.All(x =>
            x.Key.DateOfBirth >= new DateTime(1990, 1, 1) &&
            x.Key.DateOfBirth <= new DateTime(2000, 1, 1)));
        }

        [Fact]
        public void Delete_PositiveTest()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientStorage = new ClientStorage();
            var clientService = new ClientService<IClientStorage>(clientStorage);
            clientService.AddClientList(testDataGenerator.GetClientList(5));
            var client = clientStorage.Data.Keys.First();

            //Act
            clientService.DeleteClient(client);

            //Asssert
            Assert.DoesNotContain(client, clientStorage.Data.Keys);
        }

        [Fact]
        public void Update_PositiveTest()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientStorage = new ClientStorage();
            var clientService = new ClientService<IClientStorage>(clientStorage);
            clientService.AddClientList(testDataGenerator.GetClientList(5));
            var client = clientStorage.Data.Keys.First();
            var clientPassport = client.PassportNumber;

            //Act
            client.LastName = "lastName2";
            clientService.UpdateClient(client);

            //Assert
            Assert.Equal("lastName2", clientStorage.Data.First(x => x.Key.PassportNumber == clientPassport).Key.LastName);
        }

        [Fact]
        public void AddAccount_PositiveTest()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientStorage = new ClientStorage();
            var clientService = new ClientService<IClientStorage>(clientStorage);
            clientService.AddClientList(testDataGenerator.GetClientList(1000));
            var client = clientStorage.Data.First(x => !x.Value.Contains(new Account())).Key;
            var account = new Account();

            //Act
            clientService.AddAccount(client, account);

            //Assert
            Assert.Contains(account, clientStorage.Data[client]);
        }

        [Fact]
        public void UpdateAccount_PositiveTest()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientStorage = new ClientStorage();
            var clientService = new ClientService<IClientStorage>(clientStorage);
            clientService.AddClientList(testDataGenerator.GetClientList(1000));
            var client = clientStorage.Data.First().Key;
            var account = clientStorage.Data[client].First();
            account.Amount += 1000;

            //Act
            clientService.UpdateAccount(client, account);

            //Assert
            Assert.Contains(account, clientStorage.Data[client]);
        }

        [Fact]
        public void DeleteAccount_PositiveTest()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientStorage = new ClientStorage();
            var clientService = new ClientService<IClientStorage>(clientStorage);
            clientService.AddClientList(testDataGenerator.GetClientList(1000));
            var client = clientStorage.Data.First().Key;
            var account = clientStorage.Data[client].First();

            //Act
            clientService.DeleteAccount(client, account);

            //Assert
            Assert.DoesNotContain(account, clientStorage.Data[client]);
        }
    }
}
