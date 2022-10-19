using ExportTool;
using Models;
using NUnit.Framework;
using Services;
using Services.Exceptions;
using Services.Filters;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace ServiceTests
{
    public class ClientServiceTests
    {
        [Fact]
        public async Task AddClientAsync_PositiveTestAsync()
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
            await clientService.AddClientAsync(client);

            //Assert
            Assert.Equal(client, await clientService.GetClientAsync(client.Id));
        }

        [Fact]
        public async Task AddClientAsync_throwsAgeLimitException()
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
            await Assert.ThrowsAsync<AgeLimitException>(async () => await clientService.AddClientAsync(client));
        }

        [Fact]
        public async Task AddClientAsync_throwsNoPassportDataException()
        {
            //Arrange
            var clientService = new ClientService();
            var client = new Client
            {
                Id = Guid.NewGuid(),
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            //Act Assert
            await Assert.ThrowsAsync<NoPassportDataException>(async () => await clientService.AddClientAsync(client));
        }

        [Fact]
        public async Task GetClients_filteredByFirstNameAsync()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientService = new ClientService();
            await clientService.AddClientListAsync(testDataGenerator.GetClientList(100));
            var filterByFirstName = new ClientFilter()
            {
                FirstName = "Ирина"
            };

            //Act
            var filtredClients = await clientService.GetClientsAsync(filterByFirstName);

            //Assert
            Assert.True(filtredClients.All(x => x.FirstName.Contains("Ирина")));
        }

        [Fact]
        public async Task GetClients_filteredByLastNameAsync()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientService = new ClientService();
            await clientService.AddClientListAsync(testDataGenerator.GetClientList(100));
            var filterByLastName = new ClientFilter()
            {
                LastName = "Васильев"
            };

            //Act
            var filtredClients = await clientService.GetClientsAsync(filterByLastName);

            //Assert
            Assert.True(filtredClients.All(x => x.LastName.Contains("Васильев")));
        }

        [Fact]
        public async Task GetClients_filteredByPhoneNumberAsync()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientService = new ClientService();
            await clientService.AddClientListAsync(testDataGenerator.GetClientList(100));
            var filterByPhoneNumber = new ClientFilter()
            {
                PhoneNumber = 77700077
            };

            //Act
            var filtredClients = await clientService.GetClientsAsync(filterByPhoneNumber);

            //Assert
            Assert.True(filtredClients.All(x => x.PhoneNumber.ToString().Contains(77700077.ToString())));
        }

        [Fact]
        public async Task GetClients_filteredByPassportNumberAsync()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientService = new ClientService();
            await clientService.AddClientListAsync(testDataGenerator.GetClientList(100));
            var filterByPassportNumber = new ClientFilter()
            {
                PassportNumber = 900000000
            };

            //Act
            var filtredClients = await clientService.GetClientsAsync(filterByPassportNumber);

            //Assert
            Assert.True(filtredClients.All(x => x.PassportNumber.ToString().Contains(900000000.ToString())));
        }

        [Fact]
        public async Task GetClients_filteredByDateOfBirthAsync()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientService = new ClientService();
            await clientService.AddClientListAsync(testDataGenerator.GetClientList(100));
            var filterByDateOfBirth = new ClientFilter()
            {
                MinDate = new DateTime(1990, 1, 1),
                MaxDate = new DateTime(2000, 1, 1)
            };

            //Act
            var filtredClients = await clientService.GetClientsAsync(filterByDateOfBirth);

            //Assert
            Assert.True(filtredClients.All(x =>
            x.DateOfBirth >= new DateTime(1990, 1, 1) &&
            x.DateOfBirth <= new DateTime(2000, 1, 1)));
        }

        [Fact]
        public async Task GetClients_filteredWithPaginationAsync()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var clientService = new ClientService();
            await clientService.AddClientListAsync(testDataGenerator.GetClientList(100));
            var filterWithPagination = new ClientFilter()
            {
                pageNumber = 3,
                notesCount = 10
            };

            //Act
            var filtredClients = await clientService.GetClientsAsync(filterWithPagination);

            //Assert
            Assert.Equal(10, filtredClients.Count);
        }

        [Fact]
        public async Task Delete_PositiveTestAsync()
        {
            //Arrange
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
            await clientService.AddClientAsync(client);

            //Act
            await clientService.DeleteClientAsync(client.Id);

            //Asssert
            Assert.Null(await clientService.GetClientAsync(client.Id));
        }

        [Fact]
        public async Task Update_PositiveTestAsync()
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
            await clientService.AddClientAsync(client);
            client.FirstName = "Саша";

            //Act
            await clientService.UpdateClientAsync(client);

            //Assert
            var foundClient = await clientService.GetClientAsync(client.Id);
            Assert.Equal("Саша", foundClient.FirstName);
        }

        [Fact]
        public async Task AddAccount_PositiveTestAsync()
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
            await clientService.AddClientAsync(client);

            var clientList = await clientService.GetClientsAsync(new ClientFilter
            {
                PassportNumber = 900000000
            });
            client = clientList.FirstOrDefault();
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Amount = 0,
                CurrencyName = "RUP",
                ClientId = client.Id

            };

            //Act
            await clientService.AddAccountAsync(account);

            //Assert
            var foundClient = await clientService.GetClientAsync(client.Id);
            Assert.Contains(account, foundClient.AccountCollection);
        }

        [Fact]
        public async Task UpdateAccount_PositiveTestAsync()
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
            await clientService.AddClientAsync(client);
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Amount = 0,
                CurrencyName = "RUP",
                ClientId = client.Id
            };
            await clientService.AddAccountAsync(account);
            account.Amount += 100;

            //Act
            await clientService.UpdateAccountAsync(account);

            //Assert
            var foundClient = await clientService.GetClientAsync(client.Id);
            Assert.Contains(account, foundClient.AccountCollection);
        }

        [Fact]
        public async Task DeleteAccount_PositiveTestAsync()
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
            await clientService.AddClientAsync(client);
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Amount = 0,
                CurrencyName = "RUP",
                ClientId = client.Id
            };
            await clientService.AddAccountAsync(account);

            //Act
            await clientService.DeleteAccountAsync(account.Id);

            //Assert
            var foundClient = await clientService.GetClientAsync(client.Id);
            Assert.DoesNotContain(account, foundClient.AccountCollection);
        }

        [Fact]
        public async Task ImportDataFromCsv_PositiveTestAsync()
        {
            //Arrange
            var path = Path.Combine(Directory.GetCurrentDirectory(),
                "..", "..", "..", "..", "ExportTool", "ExportData");
            var list = new TestDataGenerator().GetClientList(5);
            var exportService = new ExportService();
            await exportService.ExportClientListToCsvAsync(list, path, "Clients.csv");
            var clientService = new ClientService();

            //Act
            var listFromCSV = await exportService.ReadClientListFromCsvAsync(path, "Clients.csv");
            await clientService.AddClientListAsync(listFromCSV);

            //Assert
            NUnit.Framework.Assert.That(listFromCSV,
                Is.SubsetOf(await clientService.GetClientsAsync(new ClientFilter())));
        }
    }
}
