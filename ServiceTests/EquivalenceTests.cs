using Models;
using Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace ServiceTests
{
    public class EquivalenceTests
    {
        [Fact]
        public void GetHashCodeNecessityPositivTest()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var dictionary = testDataGenerator.GetClientAndAccountDictionary(1000);
            dictionary.Add(new Client
            {
                FirstName = "firstName",
                LastName = "lastName",
                PhoneNumber = 77707000,
                PassportNumber = 900000000,
                DateOfBirth = new DateTime(2000, 1, 1)
            },
            new Account[]
            {
                new Account
                {
                    Amount = 50,
                    Currency = new Currency { Code = 840, Name = "USD" }
                }
            });
            var client = new Client
            {
                FirstName = "firstName",
                LastName = "lastName",
                PhoneNumber = 77707000,
                PassportNumber = 900000000,
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            //Act
            var accountList = dictionary[client];

            //Assert
            Assert.NotNull(accountList);
        }
        [Fact]
        public void GetHashCodeNecessityPositivTest2()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var list = testDataGenerator.GetEmployeeList(1000);
            list.Add(new Employee
            {
                FirstName = "firstName",
                LastName = "lastName",
                PhoneNumber = 77707000,
                PassportNumber = 900000000,
                DateOfBirth = new DateTime(2000, 1, 1)
            });
            var employee = new Employee
            {
                FirstName = "firstName",
                LastName = "lastName",
                PhoneNumber = 77707000,
                PassportNumber = 900000000,
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            //Act
            var searchedEmployee = list.Find(thisEmployee => thisEmployee == employee);

            //Assert
            Assert.NotNull(searchedEmployee);
        }
    }
}
