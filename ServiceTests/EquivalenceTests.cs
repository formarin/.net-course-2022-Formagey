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
            var dictionary = testDataGenerator.GetClientAndAccountDictionary();
            dictionary.Add(new Client { PhoneNumber = 77707000 }, new Account[]{
                new Account
                {
                    Amount = 50,
                    Currency = new Currency { Code = 840, Name = "USD" }
                }});
            var client = new Client { PhoneNumber = 77707000 };

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
            var list = testDataGenerator.GetEmployeeList();
            list.Add(new Employee { PhoneNumber = 77707000 });
            var employee = new Employee { PhoneNumber = 77707000 };

            //Act
            var searchedEmployee = list.Find(thisEmployee => thisEmployee == employee);

            //Assert
            Assert.NotNull(searchedEmployee);
        }
    }
}
