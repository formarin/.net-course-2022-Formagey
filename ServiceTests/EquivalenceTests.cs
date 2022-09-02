﻿using Models;
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
            Dictionary<Client, List<Account>> dictionary;
            var client = new Client { PhoneNumber = 77707000 };

            //Act
            dictionary = testDataGenerator.GetClientAndAccountDictionary();
            dictionary.Add(new Client { PhoneNumber = 77707000 }, new List<Account>{
                new Account
                {
                    Amount = 50,
                    Currency = new Currency { Code = 840, Name = "USD" }
                }});
            var accountList = dictionary[client];

            //Assert
            Assert.NotNull(accountList);
        }
        [Fact]
        public void GetHashCodeNecessityPositivTest2()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            List<Employee> list;
            var employee = new Employee { PhoneNumber = 77707000 };

            //Act
            list = testDataGenerator.GetEmployeeList();
            list.Add(new Employee { PhoneNumber = 77707000 });

            //Assert
            Assert.Contains(employee, list);
        }
    }
}
