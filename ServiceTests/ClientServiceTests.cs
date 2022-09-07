using Models;
using Services;
using Services.Exceptions;
using System;
using Xunit;

namespace ServiceTests
{
    public class ClientServiceTests
    {
        [Fact]
        public void AddClient_throwsAgeLimitException()
        {
            //Arrange
            var clientService = new ClientService();
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
            var clientService = new ClientService();
            var client = new Client
            {
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            //Act Assert
            Assert.Throws<NoPassportDataException>(() => clientService.AddClient(client));
        }

        [Fact]
        public void AddClient_throwsArgumentExceptionException()
        {
            //Arrange
            var clientService = new ClientService();
            var client = new Client
            {
                FirstName = "firstName",
                LastName = "lastName",
                PhoneNumber = 77700000,
                DateOfBirth = new DateTime(2000, 1, 1)
            };
            
            //Act
            clientService.AddClient(client);

            //Assert
            Assert.Throws<ArgumentException>(() => clientService.AddClient(client));
        }
    }
}
