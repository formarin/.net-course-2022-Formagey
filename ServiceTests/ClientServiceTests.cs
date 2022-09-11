using Models;
using Services;
using Services.Exceptions;
using System;
using Xunit;
using System.Linq;
using Services.Filters;

namespace ServiceTests
{
    public class ClientServiceTests
    {
        [Fact]
        public void AddClient_addsClient()
        {
            //Arrange
            var clientStorage = new ClientStorage();
            var clientService = new ClientService(clientStorage);
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
            Assert.Contains(client, clientStorage._clientDictionary.Keys);
        }

        [Fact]
        public void AddClient_throwsAgeLimitException()
        {
            //Arrange
            var clientService = new ClientService(new ClientStorage());
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
            var clientService = new ClientService(new ClientStorage());
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
            var clientService = new ClientService(new ClientStorage());
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
            clientStorage.Add(testDataGenerator.GetClientList());
            var clientService = new ClientService(clientStorage);
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
            clientStorage.Add(testDataGenerator.GetClientList());
            var clientService = new ClientService(clientStorage);
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
            clientStorage.Add(testDataGenerator.GetClientList());

            var clientService = new ClientService(clientStorage);
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
            clientStorage.Add(testDataGenerator.GetClientList());

            var clientService = new ClientService(clientStorage);
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
            clientStorage.Add(testDataGenerator.GetClientList());

            var clientService = new ClientService(clientStorage);
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
    }
}
